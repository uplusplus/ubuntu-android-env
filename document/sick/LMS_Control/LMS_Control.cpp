
#include "stdafx.h"
#include "LMS_Control.h"

using namespace std;
//using namespace System;

#define CRC16_GEN_POL 0x8005
#define MKSHORT(a,b) ((unsigned short) (a) | ((unsigned short)(b) << 8))


LMSControl::LMSControl()
{
	isContinuousOutput = false;
	dataLength = LEN180X5;
	range = RANGE180;
	res = RES5;
}

LMSControl::~LMSControl()
{
	
}

/***************************************************************************
 *
 * Name: ConfigureComPort
 *
 ***************************************************************************/
HANDLE LMSControl::ConfigureComPort(LPCSTR comPort)
{
		m_hCommPort = CreateFile( 
								 comPort,
								 GENERIC_ALL, 
								 0, 
								 0, 
								 OPEN_EXISTING, 
								 FILE_FLAG_NO_BUFFERING,
								 0
								);
		
		//  Check COM Port was opened successfully
		if (m_hCommPort == INVALID_HANDLE_VALUE)
		{

			if(GetLastError()==ERROR_NOT_FOUND) {
				//throw new exception("Fatal Error!!\nError Not Found");
				printf("Error Not Found - Fatal Error!");
				return 0;
			}

			printf("Cannot find the Serial Port or it may be in use by another device. Close all other applications using the port and restart this Application - Fatal Error");
			return 0;
		}

		//  SETUP COM Port
		ZeroMemory(&dcbSerialParams, sizeof(dcbSerialParams));
		dcbSerialParams.DCBlength  = sizeof(dcbSerialParams);

		if(!GetCommState(m_hCommPort, &dcbSerialParams))
		{
			printf("Unable to Retrieve Serial Port Status information - Fatal Error!");
			return 0;
			//throw new exception("Fatal Error!!\nUnable to Retrieve Serial Port Status information.");
		}

		dcbSerialParams.BaudRate				= CBR_9600;
		dcbSerialParams.ByteSize				= 8;
		dcbSerialParams.StopBits				= ONESTOPBIT;
		dcbSerialParams.Parity					= NOPARITY;


		//  Check COM Port was SETUP Correctly
		if(!SetCommState(m_hCommPort, &dcbSerialParams))
		{
			printf("Unable to Set Serial Port Status information - Fatal Error!");
			return 0;
			//throw new exception("\nFatal Error!!\nUnable to Set Serial Port Status information.");
		}

		//  Set the timeouts we control the timeout overselves using WaitForX()
		COMMTIMEOUTS timeouts;
		timeouts.ReadIntervalTimeout		    = MAXDWORD; 
		timeouts.ReadTotalTimeoutMultiplier		= 0;
		timeouts.ReadTotalTimeoutConstant		= 0;
		timeouts.WriteTotalTimeoutMultiplier	= 0;
		timeouts.WriteTotalTimeoutConstant		= 0;


		//Check Timeouts were SETUP Correctly
		if (!SetCommTimeouts(m_hCommPort, &timeouts))
		{
			printf("Unable to Set Serial Port Time-Outs - Fatal Error!");
            return 0;
			// new exception("\nFatal Error!!\nUnable to Set Serial Port Time-Outs.");
		}

		//TRY PurgeComm ON THE COM PORT TO FLUSH BUFFER
		int abRet2  = PurgeComm( m_hCommPort, PURGE_RXCLEAR );
		return m_hCommPort;
}


/***************************************************************************
 *
 * Name: ChangeAngleRes
 *
 ***************************************************************************/
void LMSControl::ChangeAngleRes(int mode)
{
	uchar buf[14];	
	
	bool wasContinuous;

	//make sure mode is between 0 and 5
	if (mode<0 || mode>5) mode = 0;


	wasContinuous = isContinuousOutput;
	if (isContinuousOutput) StopContinuousOutput();

	//Write out Angle of Resolution Telegrams to LMS

	do
	{
		SendMessage(11, ANGLE_RES[mode]);
	}
	while (!ReadLMSmsg(14, buf));


	//update object range and res variables
	switch (mode)
	{
		case ANGLE_RES_100X1:
			range = RANGE100;
			res = RES1;
			dataLength = LEN100X1;
			break;
		case ANGLE_RES_100X0_5:
			range = RANGE100;
			res = RES5;
			dataLength = LEN100X5;
			break;
		case ANGLE_RES_100X0_25:
			range = RANGE100;
			res = RES25;
			dataLength = LEN100X25;
			break;
		case ANGLE_RES_180X1:
			range = RANGE180;
			res = RES1;
			dataLength = LEN180X1;
			break;
		default: //ANGLE_RES_180X0_5
			range = RANGE180;
			res = RES5;
			dataLength = LEN180X5;
	}

	if (wasContinuous) StartContinuousOutput();

}


/***************************************************************************
 *
 * Name: ChangeBaudRate
 *
 ***************************************************************************/
void LMSControl::ChangeBaudRate(int baud)
{
	uchar buf[10];
	bool wasContinuous;

	//Make sure baud is valid
	if ((baud != BR_19200)&&(baud != BR_38400)&&(baud != BR_500000)&&(baud != BR_9600))
		baud = BR_9600;

	wasContinuous = isContinuousOutput;
	if (isContinuousOutput) StopContinuousOutput();


	do
	{
		SendMessage(8,BAUD_RATE[baud]);
	}
	while (!ReadLMSmsg(10, buf));
	
	dcbSerialParams.BaudRate=CBR[baud];

	if(!SetCommState(m_hCommPort, &dcbSerialParams))
	{
		printf("Unable to Set Serial Port Status information - Fatal Error!");
	}

	if (wasContinuous) StartContinuousOutput();

}


/***************************************************************************
 *
 * Name: StartContinuousOutput
 *
 ***************************************************************************/
void LMSControl::StartContinuousOutput()
{
	uchar buf[10];

	/*if (keepLog || keepStats)
		StartLog();*/

	//If it is not already started, start continuous output mode
	if (!isContinuousOutput)
	{
		do
		{
			SendMessage(8, START_CONTINUOUS);
		}
		while (!ReadLMSmsg(10, buf));
		isContinuousOutput = true;
	}

}



/***************************************************************************
 *
 * Name: StopContinuousOutput
 *
 ***************************************************************************/
void LMSControl::StopContinuousOutput()
{
	uchar buf[10];

	//Stop continuous output mode if it is currently in continuous mode
   	if (isContinuousOutput)
	{
		do
		{
			SendMessage(8, STOP_CONTINUOUS);
		}
		while (!ReadLMSmsg(10, buf));
		isContinuousOutput = false;
	}

}

/***************************************************************************
 *
 * Name: ReadLMSData
 *
 ***************************************************************************/
int LMSControl::ReadLMSData(int *intBuf, int coordinateSystem)
{
	// Purge buffer
	PurgeComm( m_hCommPort, PURGE_RXCLEAR );
	//unsigned short realCRC = 0;
	uchar buf[MAXPACKET];
	int tmpBuf[MAXDATA];
	int packetDataLen=0; //number of data bytes, each of them is 16bits long
	int realCRC, lenBytes;
	unsigned short CRCcalculated;

	buf[0] = 0;
	buf[1] = 0;
	buf[2] = 0;
	buf[3] = 0;
	buf[4] = 0;
	//int repeat = 0;

	while (buf[0] != STX) 
	{
		GetByte(buf[0]);
	}

	GetByte(buf[1]); // should be the ADR byte, ADR=0x80 here
	// LEN refers to the packet length in bytes from the command byte to end not including checksum
	GetByte(buf[2]); // should be the LEN low byte
	GetByte(buf[3]); // should be the LEN high byte
	GetByte(buf[4]); // Command byte 0xB0 for continuous, F5 for continuous with reflectivity

	// Check that we have a valid STX byte, valid ADR byte, and valid CMD byte
	while (
		(buf[0] != STX) || 
		(0x80 != buf[1]) || 
		(0xb0 != buf[4])
		)
	{	

		buf[0] = buf[1];
		buf[1] = buf[2];
		buf[2] = buf[3];
		buf[3] = buf[4];
		GetByte(buf[4]);
		
	}

	packetDataLen = buf[2] | ((buf[3] & 0x1f) << 8);
	//subtract 1 from packetDataLen for the cmd I already read
	packetDataLen--;
	//read in all the data in the packet except the checksum
	ReadLMS(packetDataLen,buf+5);//长度从CMD开始，长度包含状态码
	//get CRC
	GetByte(buf[packetDataLen + 5]); // should be CRC low byte*/
	GetByte(buf[packetDataLen + 6]); // should be CRC high byte*/

	realCRC = buf[packetDataLen+5] | (buf[packetDataLen+6] << 8); 
	lenBytes = packetDataLen+5;
	CRCcalculated = LMSCRC(buf, lenBytes);

	if (CRCcalculated != realCRC) 
	{
		return 0;
	}

	//convert measured values to integer array
	if (coordinateSystem == RECT)
	{
		//creates an array of integers alternating X and Y values
		ToIntArrayPolar(dataLength, buf+7, tmpBuf);
		/*
		if (keepLog || keepStats)
		{
			WriteLog(tmpBuf, dataLength);
		}
		*/
		PolarToRect(dataLength, tmpBuf, intBuf);
		return dataLength*2;
	}
	else
	{
		ToIntArrayPolar(dataLength, buf+7, intBuf);
		
		
		/*if (keepLog || keepStats)
		{
			WriteLog(intBuf, dataLength);
		}
		*/
		return dataLength;
	}	
}


/***************************************************************************
 *
 * Name: ReadLMSmsg
 *
 ***************************************************************************/
bool LMSControl::ReadLMSmsg(int len, uchar *buf)
{
	*buf = 0;
	//wait for the message header
	while(buf[0] != ACKSTX)
	{
		if (!GetByte(buf[0]))
		{
			Reconfigure();
			return false;
		}
	}
	return ReadLMS(len-1, buf+1);
}

/***************************************************************************
 *
 * Name: ReadLMS
 *
 ***************************************************************************/
bool LMSControl::ReadLMS(int len, uchar *buf)
{
	for (int i=0;i<len;i++)
	{
		if (!GetByte(buf[i]))
		{
			Reconfigure();
			return false;
		}
	}

	return true;
}

/***************************************************************************
 *
 * Name: ToIntArrayPolar
 *
 ***************************************************************************/
void LMSControl::ToIntArrayPolar(int len, uchar *buf, int *intBuf)
{
	for (int i=0;i<len;i++)
	{
		//only upper 12 bits of upper byte are used
		intBuf[i] = buf[2*i] | ((buf[2*i+1] & 0x1f) << 8);
	}
}


/***************************************************************************
 *
 * Name: GetByte
 *
 ***************************************************************************/
bool LMSControl::GetByte(uchar& c) 
{
	DWORD nBytes = 0;
	time_t finish;
	int success = 0;
	char ch= '\0';

	finish = time(NULL) + 11;
	while ((success == 0) && (time(NULL) < finish)) 
	{
		ReadFile(m_hCommPort, &ch, 1, &nBytes, NULL);
		success = nBytes;
	}

	if (success == 0) 
	{
		return false;
	}

	c = ch;
	return true;
}

/***************************************************************************
 *
 * Name: SendMessage
 *
 ***************************************************************************/
void LMSControl::SendMessage(int len, const uchar* buf) 
{
	for(int i=0;i<len;i++)
	{
		SendByte(&buf[i]);
	}
}

/***************************************************************************
 *
 * Name: SendByte
 *
 ***************************************************************************/
void LMSControl::SendByte(const uchar* c)
{

	DWORD nBytes = 0;
	time_t finish;
	int success = 0;

	finish = time(NULL) + 5;
	while ((success == 0) && (time(NULL) < finish)) {
		WriteFile(m_hCommPort, c, 1, &nBytes, NULL);
		success = nBytes;
	}

	while (success == 0) 
	{
		Reconfigure();
		finish = time(NULL) + 5;
		while ((success == 0) && (time(NULL) < finish)) 
		{
			WriteFile(m_hCommPort, c, 1, &nBytes, NULL);
			success = nBytes;
		}
	}

}


/***************************************************************************
 *
 * Name: PolarToRect
 *
 ***************************************************************************/
int LMSControl::PolarToRect(int length, int* fromArray, int* toArray)
{
	double angle;
	// set the angle of the first data element
	//first data element is the right most point relative to the LMS
	if(range == RANGE180)
	{
		angle = 0;
	}
	else
	{
		//first data point is at 40 degrees in 100 degree sweep mode
		angle = 40;
	}

	for (int i=0;i<length;i++)
	{
		//get X coordinate
		toArray[2*i] = (int)(fromArray[i]*cos(CV_PI * angle/180));
		//get Y coordinate
		toArray[2*i+1] = (int)(fromArray[i]*sin(CV_PI * angle/180));
		//increment the angle
		angle= angle+res;
	}
	return length*2;
}


int LMSControl::GetDataLength()
{
	return dataLength;
}


unsigned short LMSControl::LMSCRC(unsigned char* CommData, int lenBytes)
{
	unsigned short uCrc16;
	unsigned char abData[2];

	uCrc16 = 0;
	abData[0] = 0;

	while (lenBytes-- )
	{
		abData[1] = abData[0];
		abData[0] = *CommData++;
		if(uCrc16 & 0x8000)
		{
			uCrc16 = (uCrc16 & 0x7fff) << 1;
			uCrc16 ^= CRC16_GEN_POL;
		}
		else
		{
			uCrc16 <<= 1;
		}
		uCrc16 ^= MKSHORT (abData[0] , abData[1]);
	}
	return uCrc16;
}

void  LMSControl::Reconfigure()
{
	DWORD startBaud;
	uchar buf[MAXPACKET];
	bool success;
	//int realCRC;
	//unsigned short CRCcalculated;
	//int len;

	startBaud = dcbSerialParams.BaudRate;

	do
	{
		success = true;
		//change the baud rate
		if (dcbSerialParams.BaudRate==CBR_500000)
		{
			dcbSerialParams.BaudRate=CBR_38400;
		}
		else if(dcbSerialParams.BaudRate==CBR_9600)
		{
			dcbSerialParams.BaudRate=CBR_500000;
		}
		else if(dcbSerialParams.BaudRate==CBR_19200)
		{
			dcbSerialParams.BaudRate=CBR_9600;	
		}
		else if(dcbSerialParams.BaudRate==CBR_38400)
		{
			dcbSerialParams.BaudRate=CBR_19200;
		}

		//if I've checked all baud rates and all have failed send a message. and close the program
		if (dcbSerialParams.BaudRate == startBaud)
		{ 
			printf("Communication at all baudrates has failed. Try turning on the LMS - Communication Failure");
		}

		//set the computer serial port baud rate
		if(!SetCommState(m_hCommPort, &dcbSerialParams))
		{
			printf("Unable to set Serial Port information - Fatal Error!");
		}

		//send request for Operating Data Counter just to test communication 
		//is it a short message with a short response
		SendMessage(7,	REQ_TYPE);

		*buf = 0;
		//wait for the message header
		while(buf[0] != ACKSTX)
		{
			if (!GetByte(buf[0]))
			{
				success = false;
				break;
			}
		}
		if (success)
		{
			//get the rest of the message
			for (int i=0;i<28;i++)
			{
				if (!GetByte(buf[i]))
				{
					success = false;
					break;
				}
			}
		}

	}
	while (!success);
}


