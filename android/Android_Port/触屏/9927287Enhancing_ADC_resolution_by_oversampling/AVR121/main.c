/*! \file****************************************************************************
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
* Description       : Example of how to use oversampling to increase the resolution
*                     This code shows an example of how to increase the ADCs 
*                     resolution from 10-bit to 16-bit. It might be usefull to write 
*                     every single ADC result to UART, to ensure that LSB toggles
*                                                    
***********************************************************************************/

#include <iom16.h>                    //!< Including header file
#include <inavr.h>                    //!< Including header file

// Global variables
double        accumulator       = 0;  //!< Accumulated 10-bit samples
double        Vin               = 0;  //!< 16-bit float number result
short         temp              = 0;  //!< Temporary variable 
short         samples           = 0;  //!< Number of conversions
	
// Function prototypes
void init_adc(void);                  
void init_counter(void);              
void oversampled(void);  
             
//! ADC interrupt routine
#pragma vector=ADC_vect
__interrupt void ADCinterrupt(void)
{
  accumulator += ADC;
  samples++;
}

/! Enables ADC Interrupt enabled clk/64 AREF=VREF Single ended input
   on ADC0 Right adjusted answer */
void init_adc(void)
{
  ADCSRA = (1<<ADEN)|(1<<ADIE)|(1<<ADSC)|(1<<ADATE)|(1<<ADPS2)|(1<<ADPS1);		    											     
  ADMUX = (0<<REFS1)|(0<<REFS0);                                 
  __enable_interrupt();
}

//! Fast PWM, clk/8, Clear OC2 on compare match, 50% duty-cycle
void init_counter(void)
{
  DDRD  = (1<<PD7);                    // PD7 output for PWM-signal
  TCCR2 = (1<<WGM20)|(1<<WGM21)|(1<<COM21)|(1<<CS21);                                                
  OCR2  = 128; 
}

/*! Error compensation, Scaling 16-bit result, Rounding up
    , Calculate 16-bit result, Resets variables */
void oversampled(void)
{
  __disable_interrupt();
  accumulator += 5150;                 // Offset error compensation 
  accumulator *= 0.9993;               // Gain error compensation 
  temp=(int)accumulator%64;
  accumulator/=64;                     // Scaling the answer 
  if(temp>=32)
    {
      accumulator += 1;                // Round up 
    }
  Vin = (accumulator/65536)*4.910;     // Calculating 16-bit result
  samples     = 0;			
  accumulator = 0;			
  __enable_interrupt(); 
} 
                              
void main( void )
{
  init_adc();			       // Initiate and starts the ADC
  init_counter();                      // Initiate and starts the Counter

  while(1)                             // Eternal loop
  {
    if(samples>4095)
      {
        oversampled();              
      }
  }
}                                     

