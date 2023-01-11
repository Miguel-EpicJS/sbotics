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

void Locked(bool option)
{
    Bot.GetComponent<Servomotor>(  "lmotor"  ).Locked = option;
    Bot.GetComponent<Servomotor>(  "rmotor"  ).Locked = option;
    Bot.GetComponent<Servomotor>(  "blmotor"  ).Locked = option; 
    Bot.GetComponent<Servomotor>(  "brmotor"  ).Locked = option;
    Bot.GetComponent<Servomotor>(  "fmotor"  ).Locked = option;
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

bool VerifyUltra(string name, double lim, bool min = true) {

    if(min) {
        return ( Bot.GetComponent<UltrasonicSensor>( name ).Analog ) < lim && ( Bot.GetComponent<UltrasonicSensor>( name ).Analog ) > 0;
    } else {
        return ( Bot.GetComponent<UltrasonicSensor>( name ).Analog ) > lim && ( Bot.GetComponent<UltrasonicSensor>( name ).Analog ) != -1;
    }

}

async Task GetBall(double angle = 80, double speed = 200) {
    Locked(false);

    IO.Print($"{( Bot.Speed )} - { ( Bot.GetComponent<Servomotor>( "fmotor" ).Angle ) }");
    int count = 0;
    while( count++ < 25) {
        IO.Print($"{( Bot.Speed )} - { ( Bot.GetComponent<Servomotor>( "fmotor" ).Angle ) }");

        await Time.Delay(50);

        Locked(false);

        Bot.GetComponent<Servomotor>( "fmotor" ).Locked = true;

        Movement(-speed);

        await Time.Delay(100);

        
        Bot.GetComponent<Servomotor>( "fmotor" ).Locked = false;
        Bot.GetComponent<Servomotor>("fmotor").Apply( 300, 300 );


        Movement(speed*2);

        await Time.Delay(120);

        Locked(true);
    }
    Locked(false);
    IO.Print($"{( Bot.Speed )} - { ( Bot.GetComponent<Servomotor>( "fmotor" ).Angle ) } FF");
    Bot.GetComponent<Servomotor>( "fmotor" ).Locked = true;
}


async Task Main()
{

    Locked(false);
    await Time.Delay(75);
    Bot.GetComponent<Servomotor>(  "fmotor"  ).Locked = true;

    double speed = 150;
    double[] right = new double[2];
    double[] left  = new double[2];

    right[0] = -speed;
    right[1] = speed;

    left[0] = speed;
    left[1] = -speed;


    while(true)
    {

        await Time.Delay(50);
        Movement(speed);
        if( ( Bot.GetComponent<ColorSensor>( "lc1" ).Analog ).Brightness < 90 && (Bot.GetComponent<ColorSensor>( "rc1" ).Analog ).Brightness < 90) {
            await Time.Delay(200);
            break;
        }
    }

    while(true) {
        await Time.Delay(50);
        Movement(speed);
        if( ( Bot.GetComponent<ColorSensor>( "fs" ).Analog ).ToString() == "Preto") {
            await Time.Delay(2000);
            break;
        }
        else if(VerifyUltra("ffultra", 6)) {
            ControlMovement(right);
        }
        
    }

    while(true) {
        await Time.Delay(50);
        Movement(-speed);
        if(VerifyUltra("bultra", 1)) {
            Locked(true);
        }
        if(VerifyUltra("rultra", 18)) {

            Movement(-speed);

            await Time.Delay(600);
            await Right(speed, 90);

            while(VerifyUltra("ffultra", 4.5, false)) {
                await Time.Delay(50);
                Movement(speed);
            }
           
            await GetBall();
            await Time.Delay(4000);

            while(!(VerifyUltra("bultra", 1))) {
                await Time.Delay(50);
                Movement(-speed);
            }
            await Left(120, 90);
            Movement(speed*2);
            while( ( Bot.GetComponent<ColorSensor>( "fs" ).Analog ).ToString() != "Preto") {
                await Time.Delay(50);
            }
            Locked(false);
            await Right(speed, 90);
            await Time.Delay(2000);
        }

    }

}
