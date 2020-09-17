<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\Accessibility.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Configuration.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Deployment.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.Serialization.Formatters.Soap.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Security.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Windows.Forms.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Xaml.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\WindowsBase.dll</Reference>
  <NuGetReference>MessagePack</NuGetReference>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Net.Sockets</Namespace>
  <Namespace>System.Windows.Forms</Namespace>
  <Namespace>MessagePack</Namespace>
</Query>

string ServerIP = "127.0.0.1";
Int32 ServerPort = 32452;

CLIENT_STATE ClientState = new CLIENT_STATE();
bool IsNetworkThreadRunning = true;

ClientSimpleTcp Network = new ClientSimpleTcp();

PacketBufferManager PacketBuffer = new PacketBufferManager();

Queue<PacketData> RecvPacketQueue = new Queue<PacketData>();

Queue<byte[]> SendPacketQueue = new Queue<byte[]>();

System.Windows.Forms.Timer FormTimer = new System.Windows.Forms.Timer();
TextBox textBoxID = new TextBox();
TextBox textBoxPW = new TextBox();


void Main()
{
	PacketBuffer.Init((8096 * 10), PacketDef.PACKET_HEADER_SIZE, 1024);
	
	System.Threading.Tasks.Task.Run(() => NetworkReadProcess());
	System.Threading.Tasks.Task.Run(() => NetworkSendProcess());
		
	
	var f = CreateForm();		
	
	FormTimer.Tick += new EventHandler(PacketProcess);
	FormTimer.Interval = 250;
	FormTimer.Enabled = true; 
	
	f.Show();		
}

Form CreateForm()
{
	var f = new Form();
	f.Size = new Size(800, 600);
	f.StartPosition = FormStartPosition.CenterScreen;
	f.FormClosing += (s, e) => { 
		FormTimer.Stop(); 
		Network.Close(); 
		IsNetworkThreadRunning = false; 
		
		System.Threading.Thread.Sleep(200);
	};

	// 아이디
	f.Controls.Add(CreateLabel("아이디", new Size(55, 20), new Point(10, 10)));

	textBoxID.Size = new Size(100, 20);
	textBoxID.Location = new Point(65, 7);
	textBoxID.Text = "test1";
	f.Controls.Add(textBoxID);

	// 패스워드
	f.Controls.Add(CreateLabel("패스워드", new Size(65, 20), new Point(170, 10)));

	textBoxPW.Size = new Size(100, 20);
	textBoxPW.Location = new Point(240, 7);
	textBoxPW.Text = "123456";
	f.Controls.Add(textBoxPW);

	// 접속하기
	var btn1 = new Button();
	btn1.Size = new Size(100, 30);
	btn1.Location = new Point(10, 35);
	btn1.Text = "접속 하기";
	btn1.Click += buttonConnect_Click;
	f.Controls.Add(btn1);
	// 접속 끊기	
	var btn2 = new Button();
	btn2.Size = new Size(100, 30);
	btn2.Location = new Point(120, 35);
	btn2.Text = "접속 끊기";
	btn2.Click += (s, e) => { Network.Close(); };
	f.Controls.Add(btn2);
	// 로그인
	var btn3 = new Button();
	btn3.Size = new Size(100, 30);
	btn3.Location = new Point(230, 35);
	btn3.Text = "로그인";
	btn3.Click += buttonLogin_Click;
	f.Controls.Add(btn3);

	// 방의 유저 리스트
	// 방 입장 번호
	// 버튼 방 입장
	// 버튼 방 나가기

	// 룸 채팅 리스트
	// 채팅 메시지 입력
	// 채팅 보내기
	return f;
}



bool ConnectToServer(string ipAddress, int port)
{
	if (Network.Connect(ipAddress, port))
	{		
		$"{DateTime.Now}. 서버에 접속 중".Dump();
		return true;
	}

	$"{DateTime.Now}. 서버에 접속 실패".Dump();
	return false;
}

void CloseNetwork(ClientSimpleTcp network)
{
	network.Close();
}

void NetworkReadProcess()
{
	"NetworkReadProcess 스레드 시작".Dump();

	const Int16 PacketHeaderSize = PacketDef.PACKET_HEADER_SIZE;

	try
	{
		while (IsNetworkThreadRunning)
		{
			if (Network.IsConnected() == false)
			{
				System.Threading.Thread.Sleep(100);
				continue;
			}

			var recvData = Network.Receive();
			$"[Trace] Read. size:{recvData.Item1}".Dump();

			if (recvData != null)
			{
				PacketBuffer.Write(recvData.Item2, 0, recvData.Item1);

				while (true)
				{
					var data = PacketBuffer.Read();
					if (data.Count < 1)
					{
						$"[Trace] Pakcet을 만들크기의 데이터가 부족. size:{data.Count}".Dump();
						break;
					}

					var packet = new PacketData();
					packet.DataSize = (short)(data.Count - PacketHeaderSize);
					packet.PacketID = BitConverter.ToInt16(data.Array, data.Offset + 2);
					packet.Type = (SByte)data.Array[(data.Offset + 4)];
					packet.BodyData = new byte[packet.DataSize];
					Buffer.BlockCopy(data.Array, (data.Offset + PacketHeaderSize), packet.BodyData, 0, (data.Count - PacketHeaderSize));
					lock (((System.Collections.ICollection)RecvPacketQueue).SyncRoot)
					{
						"[Trace] RecvPacketQueue.Enqueue".Dump();
						RecvPacketQueue.Enqueue(packet);
					}
				}
				//DevLog.Write($"받은 데이터: {recvData.Item2}", LOG_LEVEL.INFO);
			}
			else
			{
				Network?.Close();
				"서버와 접속 종료 !!!".Dump();
			}
		}
	}
	catch(Exception ex)
	{
		"NetworkReadProcess 예외".Dump();
		ex.Dump();
	}
	
	"NetworkReadProcess 스레드 종료".Dump();
}

void NetworkSendProcess()
{
	"NetworkSendProcess 스레드 시작".Dump();

	try
	{
		while (IsNetworkThreadRunning)
		{
			System.Threading.Thread.Sleep(1);

			if (Network.IsConnected() == false)
			{
				continue;
			}

			lock (((System.Collections.ICollection)SendPacketQueue).SyncRoot)
			{
				if (SendPacketQueue.Count > 0)
				{
					var packet = SendPacketQueue.Dequeue();
					Network.Send(packet);
				}
			}
		}
	}
	catch(Exception ex)
	{
		"NetworkSendProcess 예외".Dump();
		ex.Dump();
	}
	
	"NetworkSendProcess 스레드 종료".Dump();
}

public void PacketProcess(object sender, EventArgs e)
{
	//Console.WriteLine(DateTime.Now);
	try
    {
    	var packet = new PacketData();

		lock (((System.Collections.ICollection)RecvPacketQueue).SyncRoot)
		{
			if (RecvPacketQueue.Count() > 0)
			{
				packet = RecvPacketQueue.Dequeue();
			}
		}

		if (packet.PacketID != 0)
		{
			PacketProcess(packet);
		}
	}
	catch (Exception ex)
	{
		MessageBox.Show(string.Format("ReadPacketQueueProcess. error:{0}", ex.Message));
	}
}

void PacketProcess(PacketData packet)
{
	switch ((PACKETID)packet.PacketID)
	{
		case PACKETID.RES_LOGIN:
			{
				var resData = MessagePackSerializer.Deserialize<PKTResLogin>(packet.BodyData);

				if (resData.Result == (short)ERROR_CODE.NONE)
				{
					ClientState = CLIENT_STATE.LOGIN;
					"로그인 성공".Dump();
				}
				else
				{
					$"로그인 실패: {resData.Result} {((ERROR_CODE)resData.Result).ToString()}".Dump();
				}
			}
			break;

		case PACKETID.RES_ROOM_ENTER:
			{
//				var resData = MessagePackSerializer.Deserialize<PKTResRoomEnter>(packet.BodyData);
//
//				if (resData.Result == (short)ERROR_CODE.NONE)
//				{
//					ClientState = CLIENT_STATE.ROOM;
//					PrintLog("방 입장 성공");
//				}
//				else
//				{
//					PrintLog(string.Format("방입장 실패: {0} {1}", resData.Result, ((ERROR_CODE)resData.Result).ToString()));
//				}
			}
			break;
		case PACKETID.NTF_ROOM_USER_LIST:
			{
//				var ntfData = MessagePackSerializer.Deserialize<PKTNtfRoomUserList>(packet.BodyData);
//
//				foreach (var user in ntfData.UserIDList)
//				{
//					listBoxRoomUserList.Items.Add(user);
//				}
			}
			break;
		case PACKETID.NTF_ROOM_NEW_USER:
			{
				//var ntfData = MessagePackSerializer.Deserialize<PKTNtfRoomNewUser>(packet.BodyData);
				//listBoxRoomUserList.Items.Add(ntfData.UserID);
			}
			break;

		case PACKETID.RES_ROOM_LEAVE:
			{
//				var resData = MessagePackSerializer.Deserialize<PKTResRoomLeave>(packet.BodyData);
//
//				if (resData.Result == (short)ERROR_CODE.NONE)
//				{
//					listBoxRoomUserList.Items.Remove(textBoxID.Text);
//					ClientState = CLIENT_STATE.LOGIN;
//					PrintLog("방 나가기 성공");
//				}
//				else
//				{
//					PrintLog(string.Format("방 나가기 실패: {0} {1}", resData.Result, ((ERROR_CODE)resData.Result).ToString()));
//				}
			}
			break;
		case PACKETID.NTF_ROOM_LEAVE_USER:
			{
				//var ntfData = MessagePackSerializer.Deserialize<PKTNtfRoomLeaveUser>(packet.BodyData);
				//listBoxRoomUserList.Items.Remove(ntfData.UserID);
			}
			break;

		case PACKETID.NTF_ROOM_CHAT:
			{
//				textBoxSendChat.Text = "";
//
//				var ntfData = MessagePackSerializer.Deserialize<PKTNtfRoomChat>(packet.BodyData);
//				listBoxChat.Items.Add($"[{ntfData.UserID}]: {ntfData.ChatMessage}");
			}
			break;
	}
}


Label CreateLabel(string text, Size size, Point point)
{
	var lbl = new Label();
	lbl.TextAlign = ContentAlignment.MiddleRight;
	lbl.Size = size;
	lbl.Location = point;
	lbl.Text = text;
	return lbl;
}


void buttonConnect_Click(object sender, EventArgs e)
{
	if(ConnectToServer(ServerIP, ServerPort))
	{
		ClientState = CLIENT_STATE.CONNECTED;
		return;
	}	
}

void buttonLogin_Click(object sender, EventArgs e)
{
	if (ClientState == CLIENT_STATE.CONNECTED)
	{
		"서버에 로그인 요청".Dump();

		var reqLogin = new PKTReqLogin() { UserID = textBoxID.Text, AuthToken = textBoxPW.Text };

		var Body = MessagePackSerializer.Serialize(reqLogin);
		var sendData = PacketToBytes.Make(PACKETID.REQ_LOGIN, Body);
		PostSendPacket(sendData);
	}
	else
	{
		System.Windows.Forms.MessageBox.Show("서버와 연결되지 않은 상태입니다");
	}
}


public void PostSendPacket(byte[] sendData)
{
	if (Network.IsConnected() == false)
	{
		"서버 연결이 되어 있지 않습니다".Dump();
		return;
	}

	SendPacketQueue.Enqueue(sendData);
}

public class PacketToBytes
{
	public static byte[] Make(PACKETID packetID, byte[] bodyData)
	{
		byte type = 0;
		var pktID = (Int16)packetID;
		Int16 bodyDataSize = 0;
		if (bodyData != null)
		{
			bodyDataSize = (Int16)bodyData.Length;
		}
		var packetSize = (Int16)(bodyDataSize + PacketDef.PACKET_HEADER_SIZE);

		var dataSource = new byte[packetSize];
		Buffer.BlockCopy(BitConverter.GetBytes(packetSize), 0, dataSource, 0, 2);
		Buffer.BlockCopy(BitConverter.GetBytes(pktID), 0, dataSource, 2, 2);
		dataSource[4] = type;

		if (bodyData != null)
		{
			Buffer.BlockCopy(bodyData, 0, dataSource, 5, bodyDataSize);
		}

		return dataSource;
	}

	public static Tuple<int, byte[]> ClientReceiveData(int recvLength, byte[] recvData)
	{
		var packetSize = BitConverter.ToInt16(recvData, 0);
		var packetID = BitConverter.ToInt16(recvData, 2);
		var bodySize = packetSize - PacketDef.PACKET_HEADER_SIZE;

		var packetBody = new byte[bodySize];
		Buffer.BlockCopy(recvData, PacketDef.PACKET_HEADER_SIZE, packetBody, 0, bodySize);

		return new Tuple<int, byte[]>(packetID, packetBody);
	}
}

enum CLIENT_STATE
{
	NONE = 0,
	CONNECTED = 1,
	LOGIN = 2,
	ROOM = 3
}

public enum ERROR_CODE : short
{
	NONE = 0, // 에러가 아니다

	// 서버 초기화 에라
	REDIS_INIT_FAIL = 1,    // Redis 초기화 에러

	// 로그인 
	LOGIN_INVALID_AUTHTOKEN = 1001, // 로그인 실패: 잘못된 인증 토큰
	ADD_USER_DUPLICATION = 1002,
	REMOVE_USER_SEARCH_FAILURE_USER_ID = 1003,
	USER_AUTH_SEARCH_FAILURE_USER_ID = 1004,
	USER_AUTH_ALREADY_SET_AUTH = 1005,
	LOGIN_ALREADY_WORKING = 1006,
	LOGIN_FULL_USER_COUNT = 1007,

	DB_LOGIN_INVALID_PASSWORD = 1011,
	DB_LOGIN_EMPTY_USER = 1012,
	DB_LOGIN_EXCEPTION = 1013,

	ROOM_ENTER_INVALID_STATE = 1021,
	ROOM_ENTER_INVALID_USER = 1022,
	ROOM_ENTER_ERROR_SYSTEM = 1023,
	ROOM_ENTER_INVALID_ROOM_NUMBER = 1024,
	ROOM_ENTER_FAIL_ADD_USER = 1025,
}

public class PacketDef
{
	public const Int16 PACKET_HEADER_SIZE = 5;
	public const int MAX_USER_ID_BYTE_LENGTH = 16;
	public const int MAX_USER_PW_BYTE_LENGTH = 16;

	public const int INVALID_ROOM_NUMBER = -1;
}

struct PacketData
{
	public Int16 DataSize;
	public Int16 PacketID;
	public SByte Type;
	public byte[] BodyData;
}

public enum PACKETID : int
{
    REQ_TEST_ECHO = 1,
    RES_TEST_ECHO = 2,
           
    // 클라이언트
    CS_BEGIN        = 1001,

    REQ_LOGIN       = 1002,
    RES_LOGIN       = 1003,
    NTF_MUST_CLOSE       = 1005,

    REQ_ROOM_ENTER = 1015,
    RES_ROOM_ENTER = 1016,
    NTF_ROOM_USER_LIST = 1017,
    NTF_ROOM_NEW_USER = 1018,

    REQ_ROOM_LEAVE = 1021,
    RES_ROOM_LEAVE = 1022,
    NTF_ROOM_LEAVE_USER = 1023,

    REQ_ROOM_CHAT = 1026,
    NTF_ROOM_CHAT = 1027,

    CS_END          = 1100,


    // 시스템, 서버 - 서버
    SS_START    = 8001,

    NTF_IN_CONNECT_CLIENT = 8011,
    NTF_IN_DISCONNECT_CLIENT = 8012,

    REQ_SS_SERVERINFO = 8021,
    RES_SS_SERVERINFO = 8023,

    REQ_IN_ROOM_ENTER = 8031,
    RES_IN_ROOM_ENTER = 8032,

    NTF_IN_ROOM_LEAVE = 8036,


    // DB 8101 ~ 9000
    REQ_DB_LOGIN        = 8101,
    RES_DB_LOGIN        = 8102,
}

// 로그인 요청
[MessagePackObject]
public class PKTReqLogin
{
	[Key(0)]
	public string UserID;
	[Key(1)]
	public string AuthToken;
}

[MessagePackObject]
public class PKTResLogin
{
	[Key(0)]
	public short Result;
}




#region PacketBufferManager
class PacketBufferManager
{
	int BufferSize = 0;
	int ReadPos = 0;
	int WritePos = 0;

	int HeaderSize = 0;
	int MaxPacketSize = 0;
	byte[] PacketData;
	byte[] PacketDataTemp;

	public bool Init(int size, int headerSize, int maxPacketSize)
	{
		if (size < (maxPacketSize * 2) || size < 1 || headerSize < 1 || maxPacketSize < 1)
		{
			return false;
		}

		BufferSize = size;
		PacketData = new byte[size];
		PacketDataTemp = new byte[size];
		HeaderSize = headerSize;
		MaxPacketSize = maxPacketSize;

		return true;
	}

	public bool Write(byte[] data, int pos, int size)
	{
		if (data == null || (data.Length < (pos + size)))
		{
			return false;
		}

		var remainBufferSize = BufferSize - WritePos;

		if (remainBufferSize < size)
		{
			return false;
		}

		Buffer.BlockCopy(data, pos, PacketData, WritePos, size);
		WritePos += size;

		if (NextFree() == false)
		{
			BufferRelocate();
		}
		return true;
	}

	public ArraySegment<byte> Read()
	{
		var enableReadSize = WritePos - ReadPos;

		if (enableReadSize < HeaderSize)
		{
			return new ArraySegment<byte>();
		}

		var packetDataSize = BitConverter.ToInt16(PacketData, ReadPos);
		if (enableReadSize < packetDataSize)
		{
			return new ArraySegment<byte>();
		}

		var completePacketData = new ArraySegment<byte>(PacketData, ReadPos, packetDataSize);
		ReadPos += packetDataSize;
		return completePacketData;
	}

	bool NextFree()
	{
		var enableWriteSize = BufferSize - WritePos;

		if (enableWriteSize < MaxPacketSize)
		{
			return false;
		}

		return true;
	}

	void BufferRelocate()
	{
		var enableReadSize = WritePos - ReadPos;

		Buffer.BlockCopy(PacketData, ReadPos, PacketDataTemp, 0, enableReadSize);
		Buffer.BlockCopy(PacketDataTemp, 0, PacketData, 0, enableReadSize);

		ReadPos = 0;
		WritePos = enableReadSize;
	}
}
#endregion

#region TCP Socket
public class ClientSimpleTcp
{
	bool IsSockConnected = false;
	public Socket Sock = null;
	public string LatestErrorMsg;
	object Lock = new Object();


	//소켓연결        
	public bool Connect(string ip, int port)
	{
		try
		{
			IPAddress serverIP = IPAddress.Parse(ip);
			int serverPort = port;

			Sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			Sock.Connect(new IPEndPoint(serverIP, serverPort));

			if (Sock == null || Sock.Connected == false)
			{
				return false;
			}

			IsSockConnected = true;
			return true;
		}
		catch (Exception ex)
		{
			LatestErrorMsg = ex.Message;
			return false;
		}
	}

	public Tuple<int, byte[]> Receive()
	{

		try
		{
			byte[] ReadBuffer = new byte[2048];
			var nRecv = Sock.Receive(ReadBuffer, 0, ReadBuffer.Length, SocketFlags.None);

			if (nRecv == 0)
			{
				return null;
			}

			return Tuple.Create(nRecv, ReadBuffer);
		}
		catch (SocketException se)
		{
			LatestErrorMsg = se.Message;
		}

		return null;
	}

	//스트림에 쓰기
	public void Send(byte[] sendData)
	{
		try
		{
			if (IsConnected()) //연결상태 유무 확인
			{
				Sock.Send(sendData, 0, sendData.Length, SocketFlags.None);
			}
			else
			{
				LatestErrorMsg = "먼저 채팅서버에 접속하세요!";
			}
		}
		catch (SocketException se)
		{
			LatestErrorMsg = se.Message;
		}
	}

	//소켓과 스트림 닫기
	public void Close()
	{
		lock (Lock)
		{
			if (IsConnected())
			{
				IsSockConnected = false;
				Sock.Close();
			}
		}
	}

	public bool IsConnected() { return (IsSockConnected && Sock != null && Sock.Connected) ? true : false; }
}
#endregion