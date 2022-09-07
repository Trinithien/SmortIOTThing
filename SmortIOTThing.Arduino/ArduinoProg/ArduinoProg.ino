void setup() {
  // put your setup code here, to run once:
  Serial.begin(10000);
}
byte b[3];
void loop() {
  // put your main code here, to run repeatedly:
  if(Serial.available()==3)
  {
    Serial.readBytes(b,3);
    
    Serial.write(b,3);
  }
}
