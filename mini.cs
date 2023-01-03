string lc1, rc1, lc2, rc2, cc;
string blc1, brc1, blc2, brc2, bcc;

string[] motors = {"lmotor", "rmotor", "blmotor", "brmotor"};

void UpdateColors()
{
    lc1 = ( Bot.GetComponent<ColorSensor>( "lc1" ).Analog ).ToString();        
    rc1 = ( Bot.GetComponent<ColorSensor>( "rc1" ).Analog ).ToString();
    lc2 = ( Bot.GetComponent<ColorSensor>( "lc2" ).Analog ).ToString();        
    rc2 = ( Bot.GetComponent<ColorSensor>( "rc2" ).Analog ).ToString();
    cc  = ( Bot.GetComponent<ColorSensor>( "cc" ).Analog ).ToString();
}

void UpdateBeforeColors()
{
    blc1 = ( Bot.GetComponent<ColorSensor>( "lc1" ).Analog ).ToString();        
    brc1 = ( Bot.GetComponent<ColorSensor>( "rc1" ).Analog ).ToString();
    blc2 = ( Bot.GetComponent<ColorSensor>( "lc2" ).Analog ).ToString();        
    brc2 = ( Bot.GetComponent<ColorSensor>( "rc2" ).Analog ).ToString();
    bcc  = ( Bot.GetComponent<ColorSensor>( "cc" ).Analog ).ToString();
}

bool IsObstacle()
{
    return ( Bot.GetComponent<UltrasonicSensor>( "ffultra" ).Analog ) > 0 && ( Bot.GetComponent<UltrasonicSensor>( "ffultra" ).Analog ) < 5;
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

void Locked(bool option)
{
    foreach(string motor in motors)
    {
        Bot.GetComponent<Servomotor>(motor).Locked = option;
    }
}

void Movement(double speed)
{
    foreach(string motor in motors)
    {
        Bot.GetComponent<Servomotor>(motor).Apply(speed, speed);
    }
}

bool VerifyObstacleDistance()
{
    return ( Bot.GetComponent<UltrasonicSensor>( "lbultra" ).Analog ) == -1 || !(( Bot.GetComponent<UltrasonicSensor>( "lbultra" ).Analog ) < 5);
}

bool VerifyObstacleExitDistance()
{
    return ( Bot.GetComponent<UltrasonicSensor>( "lbultra" ).Analog ) < 5;
}

bool VerifyTurnLeft()
{
    return ( (lc1 == "Preto" || lc2 == "Preto") && rc1 == "Branco" && rc1 == cc );
}

bool VerifyTurnRight()
{
    return ( (rc1 == "Preto" || rc2 == "Preto") && lc1 == "Branco" && lc1 == cc );
}

double ChangeSpeed(double speed)
{
    if (Bot.Speed < 1.3)
    {
        return 5;
    }        
    else if (Bot.Speed > 1.3)
    {
        return -5;
    }
    else
    {
        return 0;
    }
}

bool isExit()
{
    return ( Bot.GetComponent<ColorSensor>( "lc1" ).Analog ).Brightness < 90 && (Bot.GetComponent<ColorSensor>( "rc1" ).Analog ).Brightness < 90;
}

async Task Up()
{
    Bot.GetComponent<Servomotor>("fmotor").Locked = false;

    Bot.GetComponent<Servomotor>("fmotor").Apply(100, 100);

    await Time.Delay(100);

    Bot.GetComponent<Servomotor>("fmotor").Locked = true;
}

async Task Right(double speed, double angle) {
    double actCompass = Bot.Compass;
    while(Math.Round(Bot.Compass) != Utils.Modulo(Math.Round(actCompass+angle), 360)) {
        await Time.Delay(50);
        IO.Print($"{Math.Round(Bot.Compass)} - {Utils.Modulo(Math.Round(actCompass+angle), 360)}");
        foreach(string motor in motors )
        {
            Bot.GetComponent<Servomotor>(motor).Apply( speed, speed );
            speed = -speed;
        }
    }
}

async Task Left(double speed, double angle) {
    double actCompass = Bot.Compass;
    while(Math.Round(Bot.Compass) != Utils.Modulo(Math.Round(actCompass-angle), 360)) {
        await Time.Delay(50);
        IO.Print($"{Math.Round(Bot.Compass)} - {Utils.Modulo(Math.Round(actCompass-angle), 360)}");
        foreach(string motor in motors )
        {
            speed = -speed;
            Bot.GetComponent<Servomotor>(motor).Apply( speed, speed );
        }
    }
}

async Task ControlNegativeMovement(double speed)
{
    foreach(string motor in motors)
    {
        Bot.GetComponent<Servomotor>( motor ).Apply(speed, speed);
        speed = -speed;
    }
}


async Task Main()
{
    Locked(false);

    double speed = 100;
    double rotation = 300;

    double[] arr = new double[2];

    UpdateBeforeColors();

    while(true)
    {

        await Time.Delay(50);

        UpdateColors();

        speed += ChangeSpeed(speed);

        Movement(speed);

        if (IsGreen("lc1") && IsGreen("rc1"))
        {
            await Time.Delay(1000);
            await Right(speed, 180);


            Movement(speed);
            await Time.Delay(1000); 
        }

        if (IsGreen("lc1") && blc1 != "Preto")
        { 
            await Time.Delay(1000);
            await Left(speed, 90);


            Movement(speed);
            await Time.Delay(1000); 
        }

        if (IsGreen("rc1") && brc1 != "Preto")
        {
            await Time.Delay(1000);
            await Right(speed, 90);


            Movement(speed);
            await Time.Delay(1000); 
        }


        while ( VerifyTurnLeft() )
        {
            await Time.Delay(50);
            IO.Print("Left");
            UpdateColors();
            
            await ControlNegativeMovement( -rotation );       
        }

        while ( VerifyTurnRight() )
        {
            await Time.Delay(50);
            IO.Print("Right");
            UpdateColors();
            
            await ControlNegativeMovement( rotation );      
        }

        UpdateBeforeColors();

    }

}
