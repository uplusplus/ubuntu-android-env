/*! \file *******************************************************************
*
* Atmel Corporation
*
* File              : main.c
* Compiler          : IAR EWAAVR 2.28a/3.10c
* Revision          : $Revision: 1.1 $
* Date              : $Date: Wednesday, July 13, 2005 10:43:44 UTC $
* Updated by        : $Author: omella $
*
* Support mail      : avr@atmel.com
*
* Supported devices : All devices with a ADC can be used.
*                     The example is written for ATmega16
*
* AppNote           : AVR121 - Enhancing ADC resolution by oversampling
*
* Description       : Example of how to use oversampling to increase the resolution.
*                     10-bit result is written to UART after each conversion
*                     16-bit result are written to UART after 4096 10-bit samples
*
****************************************************************************/
#include <iom16.h>
#include <inavr.h>			

// Global variables
double        accumulator       = 0;          //!< Accumulating 10-bit samples
double        result            = 0;          //!< ASCII variable
double        Vin               = 0;          //!< 16-bit result
short         temp              = 0;          //!< Temporary memory location
short         samples           = 0;	      //!< Counting 10-bit samples

// Function prototypes
void send(unsigned char result);
void write_ascii(double accumulator);
void init_adc(void);
void init_uart(void);
void init_counter(void);
void vin(void);
void write_10bit(void);
void write_16bit(void);
void oversampled(void);
void write_space(void);

//! ADC interrupt routine
#pragma vector=ADC_vect
__interrupt void ADCinterrupt(void)
{
  accumulator += ADC;
  samples++;
}

//! Enables ADC, Interrupt enabled, ADC-clock=125kHz, VREF=AREF
void init_adc(void)
{
  ADCSRA = (1<<ADEN)|(1<<ADIE)|(1<<ADSC)|(1<<ADATE)|(1<<ADPS2)|(1<<ADPS1);
  ADMUX = (0<<REFS1)|(0<<REFS0);
  __enable_interrupt();
}

/*! \brief Initiates the UART, 19.2kbps @ 8MHz cpu-clk.,
     enables transmitter, 8 data, 1 stop, no parity
     transmitting 8 LSB */
void init_uart(void)
{
  UBRRL = 25;			
  UCSRB = (1<<TXEN);			     					
}

//! Waits for empty transmit buffer, puts data into buffer and send data
void send(unsigned char result)
{
  while (!(UCSRA & (1<<UDRE)));	
  UDR = result;         	             					
}

//! Fast PWM, Clk / 64, Clear OC2 on compare match, 50% duty cycle
void init_counter(void)
{
  DDRD  = (1<<PD7);
  TCCR2 = (1<<WGM20)|(1<<WGM21)|(1<<COM21)|(1<<CS22);
  OCR2  = 128;
}

//! Convertnumbers to ASCII and sends the result to UART
void write_ascii(double accumulator)
{
  unsigned char result;
  result = '0';
  while ( accumulator >= 10000 )
  {
    result++;
    accumulator -= 10000;
  }
  send(result);                              // Send first digit

  result = '0';
  while ( accumulator >= 1000 )
  {
    result++;
    accumulator -= 1000;
  }
  send(result);                              // Send second digit

  result = '0';
  while ( accumulator >= 100 )
  {
    result++;
    accumulator -= 100;
  }
  send(result);                              // Send third digit

  result = '0';
  while ( accumulator >= 10 )
  {
    result++;
    accumulator -= 10;
  }
  send(result);                              // Send fourth digit

  send('0' + accumulator);                   // Send fifth digit
  send('\r');                                // Send return
  send('\n');                                // Send newline
}

//! Calculating 16-bit result
void vin(void)
{
  Vin = (accumulator/65536)*4.910;	     // VREF = 4.910
}

//! Write conversion result to UART
void write_10bit(void)
{
  __disable_interrupt();
  write_ascii(ADC);
  __enable_interrupt();
}

//! Write 16-bit result to UART
void write_16bit(void)
{
  __disable_interrupt();
  write_ascii(accumulator);
  __enable_interrupt();
}

//! Write 'space' before every 16-bit result
void write_space(void)
{
  int i;
  for(i=0; i<4; i++)
    {
      send(32);                                 // Ascii value for 'space'
    }
}

/*! \brief Compensate errors, scaling result, round up,
    increase averaged result */
void oversampled(void)
{
  __disable_interrupt();
  accumulator += 5150;                         // Offset error compensation, (69*4096)/64*/
  accumulator *= 0.9993;                       // Gain error compensation*/
  temp=(int)accumulator%64;
  accumulator/=64;
  if(temp>=32)
    {
      accumulator += 1;                       //  Rounding up
    }
  write_space();
  write_16bit();
  vin();
  samples     = 0;			
  accumulator = 0;			
  __enable_interrupt();
}

void main( void )
{
  init_uart();
  init_adc();
  init_counter();

  while(1)
  {
    write_10bit();
    if(samples>4095)
      {
        oversampled();
      }
  }
}

