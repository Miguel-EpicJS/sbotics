bool Between(double val, double range1, double range2)
{
     return val >= range1 && val <= range2;
}

void Unlock()
{
    Bot.GetComponent<Servomotor>(  "lmotor"  ).Locked = false;
    Bot.GetComponent<Servomotor>(  "rmotor"  ).Locked = false;
    Bot.GetComponent<Servomotor>(  "blmotor"  ).Locked = false; 
    Bot.GetComponent<Servomotor>(  "brmotor"  ).Locked = false;
}

void Up(double vel)
{
    Bot.GetComponent<Servomotor>(  "lmotor"  ).Apply( vel, vel );
    Bot.GetComponent<Servomotor>(  "rmotor"  ).Apply( vel, vel );
    Bot.GetComponent<Servomotor>(  "blmotor"  ).Apply( vel, vel );
    Bot.GetComponent<Servomotor>(  "brmotor"  ).Apply( vel, vel );
}

void Right(double rvel)
{
    Bot.GetComponent<Servomotor>(  "lmotor"  ).Apply( rvel, rvel );
    Bot.GetComponent<Servomotor>(  "rmotor"  ).Apply( -rvel, -rvel);
    Bot.GetComponent<Servomotor>(  "blmotor"  ).Apply( rvel, rvel );
    Bot.GetComponent<Servomotor>(  "brmotor"  ).Apply( -rvel, -rvel);
}

void Left(double rvel)
{
    Right(-rvel);
}

double GetUltraSensor(string sensor)
{
    return ( Bot.GetComponent<UltrasonicSensor>( sensor ).Analog );
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

bool VerifyUltra(string name, double lim, bool min = true) {

    if(min) {
        return ( Bot.GetComponent<UltrasonicSensor>( name ).Analog ) < lim && ( Bot.GetComponent<UltrasonicSensor>( name ).Analog ) > 0;
    } else {
        return ( Bot.GetComponent<UltrasonicSensor>( name ).Analog ) > lim && ( Bot.GetComponent<UltrasonicSensor>( name ).Analog ) != -1;
    }

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

async Task OpenArms()
{

    Bot.GetComponent<Servomotor>(  "lt"  ).Locked = false;
    Bot.GetComponent<Servomotor>(  "rt"  ).Locked = false;

    Bot.GetComponent<Servomotor>(  "lt"  ).Apply( 100, 100 );
    Bot.GetComponent<Servomotor>(  "rt"  ).Apply( 100, 100 );
    
    Bot.GetComponent<Servomotor>(  "l"  ).Locked = false;
    Bot.GetComponent<Servomotor>(  "r"  ).Locked = false;

    await Time.Delay(5000);

    Bot.GetComponent<Servomotor>(  "lt"  ).Locked = true;
    Bot.GetComponent<Servomotor>(  "rt"  ).Locked = true;

    Bot.GetComponent<Servomotor>(  "l"  ).Locked = true;
    Bot.GetComponent<Servomotor>(  "r"  ).Locked = true;
}

async Task Main()
{
    Unlock();
    int vel = 180;
    int rvel = 110;

    while(true) {
        await Time.Delay(50);
        
        Up(vel);
        if (IsExit())
        {
            break;
        }

    }

    while(true) {
        await Time.Delay(50);
        Up(vel);
        if( ( Bot.GetComponent<ColorSensor>( "cc" ).Analog ).ToString() == "Preto") {
            await Time.Delay(1000);
            break;
        }
        else if(Between(GetUltraSensor("ffultra"), 1, 3)) {
            await TurnAngle(rvel, 90);
        }
    }

    int counter = 0;

    while(true) {
        await Time.Delay(50);
        Up(vel);
        if( counter == 3) {
            IO.Print("fim");
            await TurnAngle(rvel, 45);

            break;
        }
        else if(Between(GetUltraSensor("ffultra"), 1, 3)) {
            counter ++;
            await TurnAngle(rvel, 90);
        }
    }
    Up(vel);
    await OpenArms();


}
