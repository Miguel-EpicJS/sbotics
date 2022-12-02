bool vrfVerdeLeft() 
{
    return ( ( Bot.GetComponent<ColorSensor>( "lc1" ).Analog ).Green ) > ( ( Bot.GetComponent<ColorSensor>( "lc1" ).Analog ).Red ) + 15 && ( ( Bot.GetComponent<ColorSensor>( "lc1" ).Analog ).Green ) > ( ( Bot.GetComponent<ColorSensor>( "lc1" ).Analog ).Blue ) + 15;
}
bool vrfVerdeRight() 
{
    return ( ( Bot.GetComponent<ColorSensor>( "rc1" ).Analog ).Green ) > ( ( Bot.GetComponent<ColorSensor>( "rc1" ).Analog ).Red ) + 15 && ( ( Bot.GetComponent<ColorSensor>( "rc1" ).Analog ).Green ) > ( ( Bot.GetComponent<ColorSensor>( "rc1" ).Analog ).Blue ) + 15;
}

async Task Obstaculo(double rvel, double vel) 
{
    Bot.GetComponent<Servomotor>(  "lmotor"  ).Apply( 2*rvel, 2*rvel );
    Bot.GetComponent<Servomotor>(  "rmotor"  ).Apply( 2*rvel, 2*rvel - 4*rvel );
    Bot.GetComponent<Servomotor>(  "blmotor"  ).Apply( 2*rvel, 2*rvel );
    Bot.GetComponent<Servomotor>(  "brmotor"  ).Apply( 2*rvel, 2*rvel - 4*rvel );

    await Time.Delay(1000);
    
    Bot.GetComponent<Servomotor>(  "lmotor"  ).Apply( 2*vel, 2*vel );
    Bot.GetComponent<Servomotor>(  "rmotor"  ).Apply( 2*vel, 2*vel );
    Bot.GetComponent<Servomotor>(  "blmotor"  ).Apply( 2*vel, 2*vel );
    Bot.GetComponent<Servomotor>(  "brmotor"  ).Apply( 2*vel, 2*vel );

    await Time.Delay(2300);

    Bot.GetComponent<Servomotor>(  "lmotor"  ).Apply( 2*rvel, 2*rvel - 4*rvel );
    Bot.GetComponent<Servomotor>(  "rmotor"  ).Apply( 2*rvel, 2*rvel );
    Bot.GetComponent<Servomotor>(  "blmotor"  ).Apply( 2*rvel, 2*rvel - 4*rvel );
    Bot.GetComponent<Servomotor>(  "brmotor"  ).Apply( 2*rvel, 2*rvel );

    await Time.Delay(1000);

    Bot.GetComponent<Servomotor>(  "lmotor"  ).Apply( 2*vel, 2*vel );
    Bot.GetComponent<Servomotor>(  "rmotor"  ).Apply( 2*vel, 2*vel );
    Bot.GetComponent<Servomotor>(  "blmotor"  ).Apply( 2*vel, 2*vel );
    Bot.GetComponent<Servomotor>(  "brmotor"  ).Apply( 2*vel, 2*vel );

    await Time.Delay(2300);

    Bot.GetComponent<Servomotor>(  "lmotor"  ).Apply( 2*rvel, 2*rvel - 4*rvel );
    Bot.GetComponent<Servomotor>(  "rmotor"  ).Apply( 2*rvel, 2*rvel );
    Bot.GetComponent<Servomotor>(  "blmotor"  ).Apply( 2*rvel, 2*rvel - 4*rvel );
    Bot.GetComponent<Servomotor>(  "brmotor"  ).Apply( 2*rvel, 2*rvel );

    await Time.Delay(500);
    
}

async Task LevantarPa(float mult = 1)
{
    Bot.GetComponent<Servomotor>( "fmotor" ).Locked = false;
    Bot.GetComponent<Servomotor>("fmotor").Apply( 45*mult, 45*mult );
    await Time.Delay(1000);
    Bot.GetComponent<Servomotor>( "fmotor" ).Locked = true;
}

async Task Main()
{
    Bot.GetComponent<Servomotor>(  "lmotor"  ).Locked = false;
    Bot.GetComponent<Servomotor>(  "rmotor"  ).Locked = false;
    Bot.GetComponent<Servomotor>(  "blmotor"  ).Locked = false; 
    Bot.GetComponent<Servomotor>(  "brmotor"  ).Locked = false;


    double vel = 180;
    int rvel = 500;

    while(true) {
        await Time.Delay(50);
    
        Bot.GetComponent<Servomotor>(  "lmotor"  ).Apply( vel, vel );
        Bot.GetComponent<Servomotor>(  "rmotor"  ).Apply( vel, vel );
        Bot.GetComponent<Servomotor>(  "blmotor"  ).Apply( vel, vel );
        Bot.GetComponent<Servomotor>(  "brmotor"  ).Apply( vel, vel );

        if(Bot.Speed < 1.3) {
             vel += 5;
        } 
        if(Bot.Speed > 1.3) {
             vel -= 5 * (Bot.Speed - 1.3) * 10;
        }

        string lc1 = "Branco";
        string rc1 = "Branco";

        if (( Bot.GetComponent<ColorSensor>( "lc1" ).Analog ).ToString() == "Preto")
        {
            Bot.GetComponent<Light>( "led" ).TurnOn( new Color(255, 255, 255) );
            lc1 = "Preto";
        } else {
            Bot.GetComponent<Light>( "led" ).TurnOn( new Color(255, 0, 255) );
            lc1 = "Branco";
        }
        if (( Bot.GetComponent<ColorSensor>( "rc1" ).Analog ).ToString() == "Preto")
        {
            Bot.GetComponent<Light>( "led" ).TurnOn( new Color(255, 255, 255) );
            rc1 = "Preto";
        } else {
            Bot.GetComponent<Light>( "led" ).TurnOn( new Color(255, 255, 0) );
            rc1 = "Branco";
        }

        if (( Bot.GetComponent<UltrasonicSensor>("ffultra").Analog ) < 5 && ( Bot.GetComponent<UltrasonicSensor>("ffultra").Analog ) > 0) {
            await Obstaculo(rvel, vel);
        }

        while(vrfVerdeRight() && vrfVerdeLeft())
        {
            await Time.Delay(50);
            Bot.GetComponent<Light>( "led" ).TurnOn( new Color(70, 0, 0) );
            Bot.GetComponent<Servomotor>(  "lmotor"  ).Apply( 2*rvel, 2*rvel );
            Bot.GetComponent<Servomotor>(  "rmotor"  ).Apply( 2*rvel, 2*rvel - 4*rvel );
            Bot.GetComponent<Servomotor>(  "blmotor"  ).Apply( 2*rvel, 2*rvel );
            Bot.GetComponent<Servomotor>(  "brmotor"  ).Apply( 2*rvel, 2*rvel - 4*rvel );

            await Time.Delay(2300);

            Bot.GetComponent<Servomotor>(  "lmotor"  ).Apply( vel, vel );
            Bot.GetComponent<Servomotor>(  "rmotor"  ).Apply( vel, vel );
            Bot.GetComponent<Servomotor>(  "blmotor"  ).Apply( vel, vel );
            Bot.GetComponent<Servomotor>(  "brmotor"  ).Apply( vel, vel );

            await Time.Delay(1000);
        }

        while( rc1 == "Branco" && vrfVerdeRight() ) {
            await Time.Delay(50);
            Bot.GetComponent<Light>( "led" ).TurnOn( new Color(0, 0, 0) );
            Bot.GetComponent<Servomotor>(  "lmotor"  ).Apply( 2*rvel, 2*rvel );
            Bot.GetComponent<Servomotor>(  "rmotor"  ).Apply( 2*rvel, 2*rvel - 4*rvel );
            Bot.GetComponent<Servomotor>(  "blmotor"  ).Apply( 2*rvel, 2*rvel );
            Bot.GetComponent<Servomotor>(  "brmotor"  ).Apply( 2*rvel, 2*rvel - 4*rvel );

            await Time.Delay(1200);

            Bot.GetComponent<Servomotor>(  "lmotor"  ).Apply( vel, vel );
            Bot.GetComponent<Servomotor>(  "rmotor"  ).Apply( vel, vel );
            Bot.GetComponent<Servomotor>(  "blmotor"  ).Apply( vel, vel );
            Bot.GetComponent<Servomotor>(  "brmotor"  ).Apply( vel, vel );

            await Time.Delay(1000);

        }
        while( rc1 == "Branco" && vrfVerdeLeft() ) {
            await Time.Delay(50);
            Bot.GetComponent<Light>( "led" ).TurnOn( new Color(255, 255, 255) );
            Bot.GetComponent<Servomotor>(  "lmotor"  ).Apply( 2*rvel, 2*rvel - 4*rvel );
            Bot.GetComponent<Servomotor>(  "rmotor"  ).Apply( 2*rvel, 2*rvel );
            Bot.GetComponent<Servomotor>(  "blmotor"  ).Apply( 2*rvel, 2*rvel - 4*rvel );
            Bot.GetComponent<Servomotor>(  "brmotor"  ).Apply( 2*rvel, 2*rvel );

            await Time.Delay(1200);

            Bot.GetComponent<Servomotor>(  "lmotor"  ).Apply( vel, vel );
            Bot.GetComponent<Servomotor>(  "rmotor"  ).Apply( vel, vel );
            Bot.GetComponent<Servomotor>(  "blmotor"  ).Apply( vel, vel );
            Bot.GetComponent<Servomotor>(  "brmotor"  ).Apply( vel, vel );

            await Time.Delay(1000);

        }

        while( ( Bot.GetComponent<ColorSensor>( "rc1" ).Analog ).ToString() == "Preto" && ( Bot.GetComponent<ColorSensor>( "lc1" ).Analog ).ToString() != "Preto" && ( Bot.GetComponent<ColorSensor>( "cc" ).Analog ).ToString() != "Preto") {
            await Time.Delay(50);
            Bot.GetComponent<Servomotor>(  "lmotor"  ).Apply( rvel, rvel );
            Bot.GetComponent<Servomotor>(  "rmotor"  ).Apply( rvel, rvel - 2*rvel );
            Bot.GetComponent<Servomotor>(  "blmotor"  ).Apply( rvel, rvel );
            Bot.GetComponent<Servomotor>(  "brmotor"  ).Apply( rvel, rvel - 2*rvel );
        }
        while( ( Bot.GetComponent<ColorSensor>( "lc1" ).Analog ).ToString() == "Preto" && ( Bot.GetComponent<ColorSensor>( "rc1" ).Analog ).ToString() != "Preto" && ( Bot.GetComponent<ColorSensor>( "cc" ).Analog ).ToString() != "Preto") {
            await Time.Delay(50);
            Bot.GetComponent<Servomotor>(  "lmotor"  ).Apply( rvel, rvel - 2*rvel );
            Bot.GetComponent<Servomotor>(  "rmotor"  ).Apply( rvel, rvel );
            Bot.GetComponent<Servomotor>(  "blmotor"  ).Apply( rvel, rvel - 2*rvel );
            Bot.GetComponent<Servomotor>(  "brmotor"  ).Apply( rvel, rvel );

        }

        
        
    }


}
