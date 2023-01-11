string[] motors = {"lmotor", "rmotor", "blmotor", "brmotor", "flmotor", "frmotor"};
string[] leds = {"led1", "led2", "led3", "led4"};

Dictionary<string, string> SensorListAfter = new Dictionary<string, string>
{
    {"cc",  "Branco"},
    {"rc1", "Branco"},
    {"lc1", "Branco"}
};
 

void UpdateSensorListAfter()
{
    List<string> keys = new List<string>(SensorListAfter.Keys);
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

bool VerifyUltra(string name, double lim, bool min = true) {

    if(min) {
        return ( Bot.GetComponent<UltrasonicSensor>( name ).Analog ) < lim && ( Bot.GetComponent<UltrasonicSensor>( name ).Analog ) > 0;
    } else {
        return ( Bot.GetComponent<UltrasonicSensor>( name ).Analog ) > lim && ( Bot.GetComponent<UltrasonicSensor>( name ).Analog ) != -1;
    }

}

bool Between(double val, double range1, double range2)
{
     return val >= range1 && val <= range2;
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

bool IsObstacle()
{
    return ( Bot.GetComponent<UltrasonicSensor>( "ffultra" ).Analog ) > 0 && ( Bot.GetComponent<UltrasonicSensor>( "ffultra" ).Analog ) < 5;
}

bool VerifyObstacleDistance()
{
    return ( Bot.GetComponent<UltrasonicSensor>( "lultra" ).Analog ) == -1 || !(( Bot.GetComponent<UltrasonicSensor>( "lultra" ).Analog ) < 5);
}

bool VerifyObstacleExitDistance()
{
    return ( Bot.GetComponent<UltrasonicSensor>( "lultra" ).Analog ) < 5;
}

bool VerifySensorColorExit(string name)
{
    double red = ( ( Bot.GetComponent<ColorSensor>( name ).Analog ).Red );
    double green = ( ( Bot.GetComponent<ColorSensor>( name ).Analog ).Green );
    double blue = ( ( Bot.GetComponent<ColorSensor>( name ).Analog ).Blue );

    return Between(red, 50, 130) && Between(green, 50, 130) && Between(blue, 50, 130);
}

bool IsExit()
{
    return VerifySensorColorExit("rc1") && VerifySensorColorExit("lc1");
}

bool IsFlat(double inclination)
{
    return inclination > 355 || inclination < 10;
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
    double rspeed = 150;
    double maxSpeed = 3;

    //WaveFormats wave = WaveFormats.Noise;
    //Bot.GetComponent<Buzzer>("buzzer").PlaySound( 1, wave );

    while(true)
    {
        await Time.Delay(50);

        IsFlat((Bot.Inclination));

        IO.Print($"{( Bot.Inclination )} --- {Bot.Speed} -- {IsFlat(Bot.Inclination)}");
        Up(speed);

        if(IsExit())
        {
            break;
        }

        if (Bot.Speed < maxSpeed)
        {
            speed += 5;
        }
        else
        {
            speed -= 5;
        }

        if (IsObstacle() && IsFlat(Bot.Inclination))
        {
            IO.PrintLine($"{ ( Bot.GetComponent<UltrasonicSensor>( "lultra" ).Analog ) }");
            await TurnAngle(rspeed, 45);
            Up(speed);

            while (( Bot.GetComponent<UltrasonicSensor>( "lultra" ).Analog ) < 3.5 || ( Bot.GetComponent<UltrasonicSensor>( "lultra" ).Analog ) > 20)
            {
                IO.PrintLine($"{ ( Bot.GetComponent<UltrasonicSensor>( "lultra" ).Analog ) } - 1°");
                await Time.Delay(50);
            }
            await Time.Delay(500);
            while (( Bot.GetComponent<UltrasonicSensor>( "lultra" ).Analog ) > 0)
            {
                IO.PrintLine($"{ ( Bot.GetComponent<UltrasonicSensor>( "lultra" ).Analog ) } - 2°"); 
                await Time.Delay(50);
            }

            await TurnAngle(rspeed, -80);
            Up(speed);

            while(getSensorColor("rc1") != "Preto")
            {
                await Time.Delay(50);
            }
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
            await Time.Delay(700);
            await TurnAngle(rspeed, -90);


            Up(speed);
            await Time.Delay(1000); 
        }

        if (IsGreen("rc1") && SensorListAfter["rc1"] != "Preto")
        {
            await Time.Delay(700);
            await TurnAngle(rspeed, 90);


            Up(speed);
            await Time.Delay(1000); 
        }

        while(getSensorColor("cc") != "Preto" && getSensorColor("rc1") == "Preto" && IsFlat(Bot.Inclination))
        {
            await Time.Delay(50);
            Right(rspeed);
        }

        while(getSensorColor("cc") != "Preto" && getSensorColor("lc1") == "Preto" && IsFlat(Bot.Inclination))
        {
            await Time.Delay(50);
            Left(rspeed);
        }
        UpdateSensorListAfter();
        //await DragonFire();
    }
    speed = 200;


    IO.Print("2° stage");
    while(true) {
    

        await Time.Delay(50);
        Up(speed);
        if( ( Bot.GetComponent<ColorSensor>( "fs" ).Analog ).ToString() == "Preto") {
            await Time.Delay(5000);
            break;
        }
        else if(VerifyUltra("ffultra", 5)) {
            for (int i = 0; i < 9; i++)
            {
                await Time.Delay(50);
                await TurnAngle(rspeed, 10);

                Up(rspeed);
                await Time.Delay(200);
            }
            
        }
        
    }

    IO.Print("3° stage");

    while(true) {
    

        await Time.Delay(50);
        Down(speed);
        if(VerifyUltra("rmultra", 18)) {

            Down(speed);

            await Time.Delay(600);
            await TurnAngle(rspeed, 90);

            while(VerifyUltra("ffultra", 4, false)) {
                await Time.Delay(50);
                Up(speed / 2);
            }
            for (int i = 0; i < 18; i++)
            {
                await Time.Delay(50);
                await TurnAngle(rspeed, 10);

                Up(rspeed);
                await Time.Delay(200);
            }
        }
        
    }
}
