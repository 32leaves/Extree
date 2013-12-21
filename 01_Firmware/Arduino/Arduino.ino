#include <Adafruit_NeoPixel.h>

#define PIN_LEDS 6

Adafruit_NeoPixel strip = Adafruit_NeoPixel(16, PIN_LEDS, NEO_GRB + NEO_KHZ800);

void setup() {
  Serial.begin(115200);
  
  strip.begin();
  strip.show();
  //rainbowCycle(5);
}

void loop() {
  if(Serial.available()) {
    char c = Serial.read();
    switch(c) {
      case 'o': ledsOff(); break;
      case 'r': rainbowCycle(readByte()); break;
      case 's': setColors(); break;
      // case 'c': colorWipeWithColor(); break;
      case '!': strip.show(); break;
    }
  }
}

void setColors() {
  for(uint16_t i = 0; i < strip.numPixels(); i++) {
    uint8_t r = readByte();
    uint8_t g = readByte();
    uint8_t b = readByte();
    
    strip.setPixelColor(i, r, g, b);
  }
}

uint8_t readByte() {
  while(!Serial.available()) delay(1);
  return Serial.read();
}

void ledsOff() {
  for(uint16_t i = 0; i < strip.numPixels(); i++) strip.setPixelColor(i, 0);
  strip.show();
}

void colorWipe(uint32_t c, uint8_t wait) {
  for(uint16_t i = 0; i < strip.numPixels(); i++) {
      strip.setPixelColor(i, c);
      strip.show();
      delay(wait);
  }
}

void rainbow(uint8_t wait) {
  uint16_t i, j;

  for(j=0; j < 256; j++) {
    for(i=0; i<strip.numPixels(); i++) {
      strip.setPixelColor(i, Wheel((i+j) & 255));
    }
    strip.show();
    delay(wait);
  }
}

// Slightly different, this makes the rainbow equally distributed throughout
void rainbowCycle(uint8_t wait) {
  uint16_t i, j;

  for(j=0; j<256*5; j++) { // 5 cycles of all colors on wheel
    for(i=0; i< strip.numPixels(); i++) {
      strip.setPixelColor(i, Wheel(((i * 256 / strip.numPixels()) + j) & 255));
    }
    strip.show();
    delay(wait);
  }
}

// Input a value 0 to 255 to get a color value.
// The colours are a transition r - g - b - back to r.
uint32_t Wheel(byte WheelPos) {
  if(WheelPos < 85) {
   return strip.Color(WheelPos * 3, 255 - WheelPos * 3, 0);
  } else if(WheelPos < 170) {
   WheelPos -= 85;
   return strip.Color(255 - WheelPos * 3, 0, WheelPos * 3);
  } else {
   WheelPos -= 170;
   return strip.Color(0, WheelPos * 3, 255 - WheelPos * 3);
  }
}
