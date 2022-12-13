string lc1, rc1, lc2, rc2, cc;

void UpdateColors()
{
    lc1 = ( Bot.GetComponent<ColorSensor>( "lc1" ).Analog ).ToString();        
    rc1 = ( Bot.GetComponent<ColorSensor>( "rc1" ).Analog ).ToString();
    lc2 = ( Bot.GetComponent<ColorSensor>( "lc2" ).Analog ).ToString();        
    rc2 = ( Bot.GetComponent<ColorSensor>( "rc2" ).Analog ).ToString();
    cc = ( Bot.GetComponent<ColorSensor>( "cc" ).Analog ).ToString();
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

async Task Main()
{
    Locked(false);

    double speed = 100;
    double rotation = 500;

    double[] arr = new double[2];

    while(true)
    {

        await Time.Delay(50);

        if (Math.Round(Bot.Speed) < 1.4)
        {
            speed += 5;
        }        
        else if (Math.Round(Bot.Speed) > 1.4)
        {
            speed -= 5;
        }

        Movement(speed);

        UpdateColors();

        if (IsGreen("lc1"))
        {
            arr[0] = -rotation;
            arr[1] = rotation;
            await ControlMovement( arr );
            await Time.Delay(1000);

            Movement(speed);
            await Time.Delay(1000);
        }

        if (IsGreen("rc1"))
        {
            arr[0] = rotation;
            arr[1] = -rotation;
            ControlMovement( arr );
            await Time.Delay(1000);

            Movement(speed);
            await Time.Delay(1000);
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

    }

}
