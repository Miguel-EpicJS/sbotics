async Task Right(double speed, double angle) {

    double actCompass = Bot.Compass;
    while(Math.Round(Bot.Compass) != Utils.Modulo(Math.Round(actCompass+angle), 360)) {
        await Time.Delay(50);
        IO.Print( Math.Round(Bot.Compass).ToString()+ "  " + Utils.Modulo(Math.Round(actCompass+angle), 360).ToString());
        Bot.GetComponent<Servomotor>("lmotor").Apply( speed, speed );
        Bot.GetComponent<Servomotor>("rmotor").Apply( -speed, -speed );
        Bot.GetComponent<Servomotor>("blmotor").Apply( speed, speed );
        Bot.GetComponent<Servomotor>("brmotor").Apply( -speed, -speed );
    }

}

async Task Left(double speed, double angle) {

    double actCompass = Bot.Compass;
    while(Math.Round(Bot.Compass) != Utils.Modulo(Math.Round(actCompass-angle), 360)) {
        await Time.Delay(50);
        IO.Print( Math.Round(Bot.Compass).ToString()+ "  " + Utils.Modulo(Math.Round(actCompass-angle), 360).ToString());
        Bot.GetComponent<Servomotor>("lmotor").Apply( -speed, -speed );
        Bot.GetComponent<Servomotor>("rmotor").Apply( speed, speed );
        Bot.GetComponent<Servomotor>("blmotor").Apply( -speed, -speed );
        Bot.GetComponent<Servomotor>("brmotor").Apply( speed, speed );
    }

}

string lc1, rc1, lc2, rc2, cc;
string blc1, brc1, blc2, brc2, bcc;

void UpdateColors()
{
    lc1 = ( Bot.GetComponent<ColorSensor>( "lc1" ).Analog ).ToString();        
    rc1 = ( Bot.GetComponent<ColorSensor>( "rc1" ).Analog ).ToString();
    lc2 = ( Bot.GetComponent<ColorSensor>( "lc2" ).Analog ).ToString();        
    rc2 = ( Bot.GetComponent<ColorSensor>( "rc2" ).Analog ).ToString();
    cc = ( Bot.GetComponent<ColorSensor>( "cc" ).Analog ).ToString();
}

void UpdateBeforeColors()
{
    blc1 = ( Bot.GetComponent<ColorSensor>( "lc1" ).Analog ).ToString();        
    brc1 = ( Bot.GetComponent<ColorSensor>( "rc1" ).Analog ).ToString();
    blc2 = ( Bot.GetComponent<ColorSensor>( "lc2" ).Analog ).ToString();        
    brc2 = ( Bot.GetComponent<ColorSensor>( "rc2" ).Analog ).ToString();
    bcc = ( Bot.GetComponent<ColorSensor>( "cc" ).Analog ).ToString();
}

bool IsObstacle()
{
    return ( Bot.GetComponent<UltrasonicSensor>( "ffultra" ).Analog ) > 0 && ( Bot.GetComponent<UltrasonicSensor>( "ffultra" ).Analog ) < 5;
}

bool IsGreen(string name)
{
    return ( ( Bot.GetComponent<ColorSensor>( name ).Analog ).Green ) > ( ( Bot.GetComponent<ColorSensor>( name ).Analog ).Blue ) + 10 && ( ( Bot.GetComponent<ColorSensor>( name ).Analog ).Green ) > ( ( Bot.GetComponent<ColorSensor>( name ).Analog ).Red ) + 10;
}

void Locked(bool option)
{
    Bot.GetComponent<Servomotor>(  "lmotor"  ).Locked = option;
    Bot.GetComponent<Servomotor>(  "rmotor"  ).Locked = option;
    Bot.GetComponent<Servomotor>(  "blmotor"  ).Locked = option; 
    Bot.GetComponent<Servomotor>(  "brmotor"  ).Locked = option;

}

void Movement(double speed)
{
    Bot.GetComponent<Servomotor>(  "lmotor"  ).Apply(speed, speed);
    Bot.GetComponent<Servomotor>(  "rmotor"  ).Apply(speed, speed);
    Bot.GetComponent<Servomotor>(  "blmotor"  ).Apply(speed, speed); 
    Bot.GetComponent<Servomotor>(  "brmotor"  ).Apply(speed, speed);
}

async Task ControlMovement(double[] speed)
{
    Bot.GetComponent<Servomotor>(  "lmotor"  ).Apply(speed[0], speed[0]);
    Bot.GetComponent<Servomotor>(  "rmotor"  ).Apply(speed[1], speed[1]);
    Bot.GetComponent<Servomotor>(  "blmotor"  ).Apply(speed[0], speed[0]); 
    Bot.GetComponent<Servomotor>(  "brmotor"  ).Apply(speed[1], speed[1]);
}

async Task Escavator()
{
    Bot.GetComponent<Servomotor>(  "fmotor"  ).Locked = false;
    Bot.GetComponent<Servomotor>(  "fmotor"  ).Apply(100, 100);
    await Time.Delay(100);
    Bot.GetComponent<Servomotor>(  "fmotor"  ).Locked = true;
}

async Task Main()
{
    Locked(false);
    await Escavator();

    double speed = 100;
    double rotation = 500;

    double[] arr = new double[2];

    UpdateBeforeColors();

    while(true)
    {

        await Time.Delay(50);

        UpdateColors();

        if (Bot.Speed < 1.3)
        {
            speed += 5;
        }        
        else if (Bot.Speed > 1.3)
        {
            speed -= 5;
        }

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

        if (IsObstacle())
        {
            await Right(speed, 45);
            Movement(speed);
            while(( Bot.GetComponent<UltrasonicSensor>( "lbultra" ).Analog ) == -1 || !(( Bot.GetComponent<UltrasonicSensor>( "lbultra" ).Analog ) < 5))
            {
                IO.Print($"1 - {( Bot.GetComponent<UltrasonicSensor>( "lbultra" ).Analog )}");
                await Time.Delay(50);
            }
            while(( Bot.GetComponent<UltrasonicSensor>( "lbultra" ).Analog ) < 5)
            {
                IO.Print($"1 - {( Bot.GetComponent<UltrasonicSensor>( "lbultra" ).Analog )}");
                await Time.Delay(50);
            }

            await Left(speed, 90);
            Movement(speed);

            while(rc1 != "Preto")
            {
                await Time.Delay(50);
                UpdateColors();
            }
        }

        while ( ( (lc1 == "Preto" || lc2 == "Preto") && rc1 == "Branco" && rc1 == cc ) )
        {
            await Time.Delay(50);
        
            UpdateColors();
            
            arr[0] = -rotation;
            arr[1] = rotation;
            await ControlMovement( arr );
        }

        while ( ( (rc1 == "Preto" || rc2 == "Preto") && lc1 == "Branco" && lc1 == cc ) )
        {
            await Time.Delay(50);

            UpdateColors();
            
            arr[0] = rotation;
            arr[1] = -rotation;
            await ControlMovement( arr );
        }

        UpdateBeforeColors();

    }

}
