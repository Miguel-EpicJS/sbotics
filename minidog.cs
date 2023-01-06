string[] motors = {"lmotor", "rmotor", "blmotor", "brmotor"};
string[] leds = {"led1", "led2", "led3", "led4"};

Dictionary<string, string> SensorListAfter = new Dictionary<string, string>
{
    {"cc",  "Branco"},
    {"rc1", "Branco"},
    {"lc1", "Branco"}
};
 

void UpdateSensorListAfter()
{
    Dictionary<string, string>.KeyCollection keys = SensorListAfter.Keys; 
    foreach(string key in keys)
    {
        SensorListAfter[key] = getSensorColor(key);
    }
}

void Locked(bool option)
{
    foreach(string motor in motors)
    {
        Bot.GetComponent<Servomotor>(motor).Locked = option;
    }
}

void Up(double speed)
{
    foreach(string motor in motors)
    {
        Bot.GetComponent<Servomotor>(motor).Apply(speed, speed);
    }
}

void Down(double speed)
{
    speed = -speed;
    foreach(string motor in motors)
    {
        Bot.GetComponent<Servomotor>(motor).Apply(speed, speed);
    }
}

void Right(double speed)
{
    foreach(string motor in motors)
    {
        Bot.GetComponent<Servomotor>(motor).Apply(speed, speed);
        speed = -speed;
    }
}

void Left(double speed)
{
    foreach(string motor in motors)
    {
        speed = -speed;
        Bot.GetComponent<Servomotor>(motor).Apply(speed, speed);
    }
}

void LedColor(Color cor)
{
    foreach(string led in leds)
    {
        Bot.GetComponent<Light>(led).TurnOn( cor );
    }
}

string getSensorColor(string sensorName)
{
    return ( Bot.GetComponent<ColorSensor>(sensorName).Analog ).ToString();
}

bool IsGreenForBlue(string name)
{
    return ( ( Bot.GetComponent<ColorSensor>( name ).Analog ).Green ) > ( ( Bot.GetComponent<ColorSensor>( name ).Analog ).Blue ) + 10;
}

bool IsGreenForRed(string name)
{
    return ( ( Bot.GetComponent<ColorSensor>( name ).Analog ).Green ) > ( ( Bot.GetComponent<ColorSensor>( name ).Analog ).Red ) + 10;
}

bool IsGreen(string name)
{
    return IsGreenForBlue(name) && IsGreenForRed(name);
}

async Task DragonFire()
{
    LedColor(new Color(255, 0, 0));
    await Time.Delay(700);
    LedColor(new Color(255, 150, 0));
    await Time.Delay(700);
    LedColor(new Color(255, 255, 0));
    await Time.Delay(700);
}

async Task TurnAngle(double speed, double angle)
{
    double compass = Bot.Compass;
    while( Math.Round(Bot.Compass) != Utils.Modulo(Math.Round(compass+angle), 360) ) {

        await Time.Delay(50);

        if (angle > 0)
        {
            Right(speed);
        }
        else
        {
            Left(speed);
        }

    }
}

async Task Main()
{
    Locked(false);

    double speed = 200;
    double rspeed = 100;
    double maxSpeed = 1.6;

    //WaveFormats wave = WaveFormats.Noise;
    //Bot.GetComponent<Buzzer>("buzzer").PlaySound( 1, wave );

    while(true)
    {
        await Time.Delay(50);

        Up(speed);

        if (Bot.Speed < maxSpeed)
        {
            speed += 5;
        }
        else
        {
            speed -= 5;
        }

        if (IsGreen("lc1") && IsGreen("rc1"))
        {
            await Time.Delay(1000);
            await TurnAngle(rspeed, 180);


            Up(speed);
            await Time.Delay(1000); 
        }

        if (IsGreen("lc1") && SensorListAfter["lc1"] != "Preto")
        { 
            await Time.Delay(1000);
            await TurnAngle(rspeed, -90);


            Up(speed);
            await Time.Delay(1000); 
        }

        if (IsGreen("rc1") && SensorListAfter["rc1"] != "Preto")
        {
            await Time.Delay(1000);
            await TurnAngle(rspeed, 90);


            Up(speed);
            await Time.Delay(1000); 
        }

        while(getSensorColor("cc") != "Preto" && getSensorColor("rc1") == "Preto")
        {
            await Time.Delay(50);
            Right(speed);
        }

        while(getSensorColor("cc") != "Preto" && getSensorColor("lc1") == "Preto")
        {
            await Time.Delay(50);
            Left(speed);
        }
        UpdateSensorListAfter();
        //await DragonFire();
    }

}
