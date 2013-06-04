sudo apt-get install gedit-plugins 
其它插件https://live.gnome.org/Gedit/Plugins
apt-fast install ctags

安装Apt-fast：
sudo add-apt-repository ppa:apt-fast/stable
sudo apt-get update
sudo apt-get install apt-fast

eclipse 市场安装：http://download.eclipse.org/mpc/indigo/

安装svn可视化插件:http://wiki.rabbitvcs.org/wiki/install/ubuntu
sudo add-apt-repository ppa:rabbitvcs/ppa
apt-get install rabbitvcs-nautilus3


gedit 显示中文乱码
gsettings set org.gnome.gedit.preferences.encodings auto-detected "['GB18030', 'UTF-8', 'GB2312', 'GBK', 'BIG5', 'CURRENT', 'UTF-16']"

设置无线优先：
#!/bin/sh  
# /home/trevor/cpplive/WifiGateway.sh  
wifi=`iwconfig | head -1 | awk '{print $1}'`  
echo "Wifi interface: $wifi"  
gw=`ip addr show $wifi | head -3 | tail -1 | awk '{print $2}'`  
echo "Wifi IP: "$gw  
gw=`echo $gw | awk -F. '{printf("%s.%s.%s.1",$1,$2,$3)}'`  
echo "Wifi gateway: "$gw  
echo "Set the Wifi gateway as the default gw now"  
route delete default  
route add default gw $gw  
echo "Set the Wifi gateway as the default gw end"  

修改gedit插件terminal背景
gsettings set org.gnome.gedit.plugins.terminal background-color '#FFFFDD'
 

eclipse 安装插件 http://eclipsecolorthemes.org/
