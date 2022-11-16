
const int hot = 30; //hot parameter
const int cold = 15; //cold parameter
const unsigned long long intervalTemp = 1000;
unsigned long long prevTempTime=0;

void setup() {
  pinMode(A0, INPUT); //tmpSensor
  pinMode(2, OUTPUT); //redLedDiode
  pinMode(3, OUTPUT); //redLedDiode
  Serial.begin(9600);
}

void tempSensor() {
  float sensor = analogRead(A0); //Reads the value of the analog pin
  float voltage = (sensor/1024)*5000; //Sensor output range is 0.1-2.0V, 1024bits over 1.9V and times 5 to get a range between 0-5volt
  if(voltage < 0.0001)
  {
      voltage = 0.1;
  }
  if(voltage > 2000.0)
  {
      voltage = 2.0;
  }
  float tempC = (voltage-500)/10; //To convert from voltage to temprature
  
  //Serial.print("TT: "); //Alarm for too cold temp
  //Serial.println(tempC);
  if (tempC < cold) { //cold
    digitalWrite(2, HIGH); 
    digitalWrite(3, LOW);

  }
  else if (tempC >= hot) { //Alarm for too hot temp
    digitalWrite(2, LOW);
    digitalWrite(3, HIGH);
  }
  else { //fine
    digitalWrite(2, LOW); //Temp is perfect
    digitalWrite(3, LOW);
  }

  unsigned long long curTempTime = millis();

  if (curTempTime - prevTempTime >= intervalTemp){
    Serial.println(tempC);
    prevTempTime = curTempTime;
  }
}

void loop(){
  tempSensor();
}
