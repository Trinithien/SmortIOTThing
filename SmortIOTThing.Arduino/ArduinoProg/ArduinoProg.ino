
const int hot = 25; //hot parameter
const int cold = 20; //cold parameter
void setup() {
  pinMode(A0, INPUT); //tmpSensor
  pinMode(2, OUTPUT); //redLedDiode
  pinMode(3, OUTPUT); //redLedDiode
  Serial.begin(9600);
}

void tempSensor() {
  float sensor = analogRead(A0); //Reads the value of the analog pin
  float voltage = (sensor/1024)*5000; //Sensor output range is 0.1-2.0V, 1024bits over 1.9V and times 5 to get a range between 0-5volt
  if(voltage < 0.1)
  {
      voltage = 0.1;
  }
  if(voltage > 2.0)
  {
      voltage = 2.0;
  }
  float tempC = (voltage-500)/10; //To convert from voltage to temprature
  
  //Serial.print("TT: "); //Alarm for too cold temp
  //Serial.println(tempC);
  if (tempC < cold) { //cold
    digitalWrite(2, HIGH); 
    digitalWrite(3, LOW);
    char sz[] = "";
    char ssz[] = "";
    dtostrf(tempC,0,2,sz);
    Serial.println(sz);
    sprintf(ssz,"TT:%s",sz);
    Serial.println(ssz);
    Serial.println(tempC);
  }
  else if (tempC >= hot) { //Alarm for too hot temp
    digitalWrite(2, LOW);
    digitalWrite(3, HIGH);
    //Serial.println(tempC);
  }
  else { //fine
    digitalWrite(2, LOW); //Temp is perfect
    digitalWrite(3, LOW);
    //Serial.println(tempC);
  }
  delay(1000);
}

void loop(){
  tempSensor();
}
