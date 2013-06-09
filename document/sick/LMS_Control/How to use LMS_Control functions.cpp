#define COM_PORT 1

//
// LIDAR Initialization
//
LMSControl Laser;
char s[5];
sprintf(s, "COM%d", COM_PORT);
Laser.ConfigureComPort((LPCSTR)s);
Laser.ChangeBaudRate(BR_500000);
Laser.ChangeAngleRes(ANGLE_RES_100X0_25); 
Laser.StartContinuousOutput();

//
// Retrieve latest LIDAR scan
//
int lidar[NUM_LIDAR_POINTS];
Laser.ReadLMSData(lidar, POLAR);

//
// Convert from polar to Cartesian coordinates using openCV
//
CvMat* xpoints = cvCreateMat(NUM_LIDAR_POINTS, 1, CV_32FC1);
CvMat* ypoints = cvCreateMat(NUM_LIDAR_POINTS, 1, CV_32FC1);
CvMat* polar_mags = cvCreateMat(NUM_LIDAR_POINTS, 1, CV_32FC1);
CvMat* polar_angs = cvCreateMat(NUM_LIDAR_POINTS, 1, CV_32FC1);
for(int a = 0; a < NUM_LIDAR_POINTS; a++)
{
	if(NUM_LIDAR_POINTS % 180 == 1)
		CV_MAT_ELEM(*polar_angs, float, a, 0) = (float)a / (float)((NUM_LIDAR_POINTS-1)/180);
	else
		// Center 100 degree scans around +90 degrees for conversion
		CV_MAT_ELEM(*polar_angs, float, a, 0) = (float)40 + (float)a / (float)((NUM_LIDAR_POINTS-1)/100);
	CV_MAT_ELEM(*polar_mags, float, a, 0) = (float)lidar[a]/3;
}
cvPolarToCart(polar_mags, polar_angs, xpoints, ypoints, 1);
