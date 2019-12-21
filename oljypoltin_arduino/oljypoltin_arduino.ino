
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
  delay(1000);
  
  if (StatoSwitch == HIGH)
  
  {
    
    Serial.println("1");
    digitalWrite(pinLed, HIGH);
  
  }
  
  else
  
  {
    Serial.println("0");
    digitalWrite(pinLed, LOW);

  }

}
