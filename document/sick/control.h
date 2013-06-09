// 停止水平转台
#define SENDMSGSTOP #STOP@
#define RECVMSGSTOP #HALT!@

// 返回步数
#define SENDMSGREST #REST@
#define RECVMSGREST #%05d!@

// 返回温度
#define SENDMSGRETE #RETE@
#define RECVMSGRETE #%02.1f@

// 返回水平倾角
#define SENDMSGREAN #REAN@
#define RECVMSGREAN #%5d!%5d!@

// 相机拍照
#define SENDMSGTAPH #TAPH@
#define RECVMSGTAPH #PHOTOOK!@

// 亮绿灯
#define SENDMSGLEDR #LEDR@
#define RECVMSGLEDR #LEDROK!@

// 亮红灯
#define SENDMSGLEDG #LEDG@
#define RECVMSGLEDG #LEDROK!@

// 熄灯
#define SENDMSGLEDX #LEDX@
#define RECVMSGLEDX #LEDXOK!@

// 搜索水平转台零点
#define SENDMSGSEZE #SEZE@
#define RECVMSGSEZE #SEZEOK!@

// 返回公司名称
#define SENDMSGREMF #REMF@
#define RECVMSGREPN #B9E3D6DDD6D0BAA3B4EFCEC0D0C7B5BCBABDBCBCCAF5B9C9B7DDD3D0CFDEB9ABCBBE@

// 返回产品型号
#define SENDMSGREMO #REMO@
#define RECVMSGREMO #LS300@

// 返回产品编号
#define SENDMSGRESN #RESN@
#define RECVMSGRESN #LS300-%03d@

// 返回生成日期
#define SENDMSGREPD #REPD@
#define RECVMSGREPD #%4d.%02d.%02d@

// 返回硬件IP地址
#define SENDMSGREIP #REIP@
#define RECVMSGREIP #192.168.1.10@

// 关机提示
#define SENDMSGFM #FM@

// 水平转台转动(#MOTO,正转/反转,步数,速度@)
#define SENDMSGMOTO #MOTO,R/F,%05d,%04d@
#define RECVMSGMOTO #MOTOOK!@