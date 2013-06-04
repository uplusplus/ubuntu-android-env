#include <exception>
#include <iostream>

class exception_test{
	public:
	void throw_exception(){
		throw std::exception();
	}

};


int main(int argc,char**argv){
	exception_test et;
	getchar();
	try{	
		et.throw_exception();
	}catch(std::exception & e){
		printf("got a exception\r\n");
	}
}