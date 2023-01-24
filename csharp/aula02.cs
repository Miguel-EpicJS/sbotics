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
    Bot.GetComponent<Servomotor>(  "rmotor"  ).Apply( rvel, rvel - 2*rvel );
    Bot.GetComponent<Servomotor>(  "blmotor"  ).Apply( rvel, rvel );
    Bot.GetComponent<Servomotor>(  "brmotor"  ).Apply( rvel, rvel - 2*rvel );
}

void Left(double rvel)
{
    Right(-rvel);
}

bool DetectCurve(string sensor)
{
    return ( Bot.GetComponent<ColorSensor>( sensor ).Analog ).ToString() == "Preto" && ( Bot.GetComponent<ColorSensor>( "cc" ).Analog ).ToString() != "Preto";
}

bool DetectGreen(string sensor)
{
    return ( ( Bot.GetComponent<ColorSensor>( sensor ).Analog ).Green ) > ( ( Bot.GetComponent<ColorSensor>( sensor ).Analog ).Blue ) + 10 && ( ( Bot.GetComponent<ColorSensor>( sensor ).Analog ).Green ) > ( ( Bot.GetComponent<ColorSensor>( sensor ).Analog ).Red ) + 10;
}

string GetColorSensor(string sensor)
{
    return ( Bot.GetComponent<ColorSensor>( sensor ).Analog ).ToString();
}

double GetUltraSensor(string sensor)
{
    return ( Bot.GetComponent<UltrasonicSensor>( sensor ).Analog );
}

async Task AlignItself(double rvel)
{
    while (GetColorSensor("cc") != "Preto")
    {
        await Time.Delay(50);
        Right(rvel);
    }
   
}

async Task Main()
{
    Unlock();
    int vel = 180;
    int rvel = 300;

    bool rc1 = false;
    bool lc1 = false;

    while(true) {
        await Time.Delay(50);
        
        Up(vel);

        if (GetUltraSensor("ffultra") < 5 && GetUltraSensor("ffultra") != -1)
        {
            Up(vel);
            await Time.Delay(1000);

            if (GetUltraSensor("ffultra") < 5 && GetUltraSensor("ffultra") != -1)
            {
                Up(-vel);
                await Time.Delay(1000);
                Right(rvel);
                while (GetColorSensor("lc2") != "Preto")
                {
                    await Time.Delay(50);
                }
                Up(vel);
                while ((GetUltraSensor("lc2")) < 3 )
                {
                    await Time.Delay(50);
                }
                Up(vel);
                await Time.Delay(1000);
                Right(rvel);
                await Time.Delay(3000);
            }
        }

        if (DetectGreen("rc1") && rc1 && DetectGreen("lc1") && lc1)
        {
            Up(-vel);
            await Time.Delay(500);
            Right(rvel);
            await Time.Delay(1000);

            await AlignItself(rvel);

        }

        if (( DetectGreen("rc1") || DetectGreen("rc2") ) && rc1)
        {
            Right(rvel*2);
            await Time.Delay(50);

            await AlignItself(rvel);
        }

        if (( DetectGreen("lc1") || DetectGreen("lc2") ) && lc1)
        {
            Left(rvel*2);
            await Time.Delay(50);

            await AlignItself(rvel);
        }

        while( DetectCurve("rc1")) {
            await Time.Delay(50);

            Right(rvel);

        }
        while( DetectCurve("lc1")) {
            await Time.Delay(50);

            Left(rvel);

        }

        rc1 = GetColorSensor("rc1") == "Preto" ? false : true;
        lc1 = GetColorSensor("lc1") == "Preto" ? false : true;

    }


}
