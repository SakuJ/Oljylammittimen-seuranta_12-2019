
const int pinSwitch = 12; //Pin Reed

const int pinLed = 9; //Pin LED


int StatoSwitch = 0;

void setup()

{
  Serial.begin(9600);
  
  pinMode(pinLed, OUTPUT);
  
  pinMode(pinSwitch, INPUT);

}

void loop()

{

  StatoSwitch = digitalRead(pinSwitch);
  
  int sensorValue = analogRead(A0);
  float voltage = sensorValue * (5.0 / 1023.0);
  
  float Rntc = 10*((5/voltage)-1);

  float t = (1.042*pow(10.0f,-5.0f)*pow(Rntc,6.0f)) - (9.749*pow(10.0f,-4.0f)*pow(Rntc,5.0f)) +
        (3.608*pow(10.0f,-2.0f)*pow(Rntc,4.0f)) - (6.751*pow(10.0f,-1.0f)*pow(Rntc,3.0f)) +
        (6.808*pow(Rntc,2.0f)) - (3.803*10*Rntc) + (1.272*pow(10.0f,2.0f));
  
  delay(1000);
  
  if (StatoSwitch == HIGH)
  
  {
    
    Serial.print("1,");
    Serial.println(t);
    digitalWrite(pinLed, HIGH);
  
  }
  
  else
  
  {
    Serial.print("0,");
    Serial.println(t);
    digitalWrite(pinLed, LOW);

  }
}
