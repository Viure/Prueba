'
' OpenTok .NET Library
' Last Updated November 16, 2011
' https://github.com/opentok/Opentok-.NET-SDK
'

Imports System
Imports System.Collections.Generic
Imports System.Xml
Imports System.Collections.Specialized
Imports System.Configuration
Imports System.Text
Imports System.Net
Imports System.Web
Imports System.Security.Cryptography
Imports System.IO

Namespace OpenTok
    Public Class OpenTokSDK
        Public Function CreateSession(location As String) As String
            Dim options As New Dictionary(Of String, Object)()

            Return CreateSession(location, options)
        End Function

        Public Function CreateSession(location As String, options As Dictionary(Of String, Object)) As String
            Dim appSettings As NameValueCollection = ConfigurationManager.AppSettings
            options.Add("location", location)
            options.Add("partner_id", appSettings("opentok_key"))

            Dim xmlDoc As XmlDocument = CreateSessionId(String.Format("{0}/session/create", appSettings("opentok_server")), options)

            Dim session_id As String = xmlDoc.GetElementsByTagName("session_id")(0).ChildNodes(0).Value

            Return session_id
        End Function

        Public Function GenerateToken(sessionId As String) As String
            Dim options As New Dictionary(Of String, Object)()

            Return GenerateToken(sessionId, options)
        End Function

        Public Function GenerateToken(sessionId As String, options As Dictionary(Of String, Object)) As String
            Dim appSettings As NameValueCollection = ConfigurationManager.AppSettings

            options.Add("session_id", sessionId)
            options.Add("createTime", (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000L) \ 10000000)
            options.Add("nonce", RandomNumber(0, 999999))
            If Not options.ContainsKey(TokenPropertyConstants.ROLE) Then
                options.Add(TokenPropertyConstants.ROLE, "publisher")
            End If
            ' Convert expire time to Unix Timestamp
            If options.ContainsKey(TokenPropertyConstants.EXPIRE_TIME) Then
                Dim origin As New DateTime(1970, 1, 1, 0, 0, 0)
                Dim expireTime As DateTime = CType(options(TokenPropertyConstants.EXPIRE_TIME), DateTime)
                Dim diff As TimeSpan = expireTime - origin
                options(TokenPropertyConstants.EXPIRE_TIME) = Math.Floor(diff.TotalSeconds)
            End If

            Dim dataString As String = String.Empty
            For Each pair As KeyValuePair(Of String, Object) In options
                dataString += pair.Key & "=" & HttpUtility.UrlEncode(pair.Value.ToString()) & "&"
            Next

            Dim sig As String = SignString(dataString, appSettings("opentok_secret").Trim())
            Dim token As String = String.Format("{0}{1}", appSettings("opentok_token_sentinel"), EncodeTo64(String.Format("partner_id={0}&sdk_version={1}&sig={2}:{3}", appSettings("opentok_key"), appSettings("opentok_sdk_version"), sig, dataString)))

            Return token
        End Function

        Private Shared Function EncodeTo64(data As String) As String
            Dim encData_byte As Byte() = New Byte(data.Length - 1) {}
            encData_byte = Encoding.UTF8.GetBytes(data)
            Dim encodedData As String = Convert.ToBase64String(encData_byte)

            Return encodedData
        End Function

        Private Function RandomNumber(min As Integer, max As Integer) As Integer
            Dim random As New Random()
            Return random.[Next](min, max)
        End Function

        Private Function SignString(message As String, key As String) As String
            Dim encoding As New ASCIIEncoding()

            Dim keyByte As Byte() = encoding.GetBytes(key)

            Dim hmacsha1 As New HMACSHA1(keyByte)

            Dim messageBytes As Byte() = encoding.GetBytes(message)
            Dim hashmessage As Byte() = hmacsha1.ComputeHash(messageBytes)

            'Make sure to utilize ToLower() method, else an exception willl be thrown
            'Exception: 1006::Connecting to server to fetch session info failed.
            Dim result As String = ByteToString(hashmessage).ToLower()

            Return result
        End Function

        Private Shared Function ByteToString(buff As Byte()) As String
            Dim sbinary As String = ""

            For i As Integer = 0 To buff.Length - 1
                'Hex format
                sbinary += buff(i).ToString("X2")
            Next
            Return (sbinary)
        End Function

        Private Function CreateSessionId(uri As String, dict As Dictionary(Of String, Object)) As XmlDocument
            Dim xmlDoc As New XmlDocument()
            Dim appSettings As NameValueCollection = ConfigurationManager.AppSettings

            Dim postData As String = String.Empty
            For Each pair As KeyValuePair(Of String, Object) In dict
                postData += pair.Key & "=" & HttpUtility.UrlEncode(pair.Value.ToString()) & "&"
            Next
            postData = postData.Substring(0, postData.Length - 1)
            Dim postBytes As Byte() = Encoding.UTF8.GetBytes(postData)

            Dim request As HttpWebRequest = DirectCast(WebRequest.Create(uri), HttpWebRequest)
            request.KeepAlive = False
            request.ProtocolVersion = HttpVersion.Version10
            request.Method = "POST"
            request.ContentType = "application/x-www-form-urlencoded"
            request.ContentLength = postBytes.Length
            request.Headers.Add("X-TB-PARTNER-AUTH", String.Format("{0}:{1}", appSettings("opentok_key"), appSettings("opentok_secret").Trim()))

            Dim requestStream As Stream = request.GetRequestStream()

            requestStream.Write(postBytes, 0, postBytes.Length)
            requestStream.Close()

            Using response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
                If response.StatusCode = HttpStatusCode.OK Then
                    Using reader As XmlReader = XmlReader.Create(response.GetResponseStream(), New XmlReaderSettings())
                        xmlDoc.Load(reader)
                    End Using
                End If
            End Using

            Return xmlDoc
        End Function
    End Class

    Public Class SessionPropertyConstants
        Public Const ECHOSUPRESSION_ENABLED As String = "echoSuppression.enabled"
        Public Const MULTIPLEXER_NUMOUTPUTSTREAMS As String = "multiplexer.numOutputStreams"
        Public Const MULTIPLEXER_SWITCHTYPE As String = "multiplexer.switchType"
        Public Const MULTIPLEXER_SWITCHTIMEOUT As String = "multiplexer.switchTimeout"
        Public Const P2P_PREFERENCE As String = "p2p.preference"
    End Class

    Public Class TokenPropertyConstants
        Public Const ROLE As String = "role"
        Public Const EXPIRE_TIME As String = "expire_time"
        Public Const CONNECTION_DATA As String = "connection_data"
    End Class

    Public Class RoleConstants
        Public Const SUBSCRIBER As String = "subscriber"
        Public Const PUBLISHER As String = "publisher"
        Public Const MODERATOR As String = "moderator"
    End Class
End Namespace
