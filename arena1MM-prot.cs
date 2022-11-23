bool vrfVerdeLeft() 
{
    return ( ( Bot.GetComponent<ColorSensor>( "lc1" ).Analog ).Green ) > ( ( Bot.GetComponent<ColorSensor>( "lc1" ).Analog ).Red ) + 15 && ( ( Bot.GetComponent<ColorSensor>( "lc1" ).Analog ).Green ) > ( ( Bot.GetComponent<ColorSensor>( "lc1" ).Analog ).Blue ) + 15;
}
bool vrfVerdeRight() 
{
    return ( ( Bot.GetComponent<ColorSensor>( "rc1" ).Analog ).Green ) > ( ( Bot.GetComponent<ColorSensor>( "rc1" ).Analog ).Red ) + 15 && ( ( Bot.GetComponent<ColorSensor>( "rc1" ).Analog ).Green ) > ( ( Bot.GetComponent<ColorSensor>( "rc1" ).Analog ).Blue ) + 15;
}

async Task Main()
{
    Bot.GetComponent<Servomotor>(  "lmotor"  ).Locked = false;
    Bot.GetComponent<Servomotor>(  "rmotor"  ).Locked = false;
    Bot.GetComponent<Servomotor>(  "blmotor"  ).Locked = false; 
    Bot.GetComponent<Servomotor>(  "brmotor"  ).Locked = false;

    int vel = 180;
    int rvel = 500;

    while(true) {
        await Time.Delay(50);
    
        Bot.GetComponent<Servomotor>(  "lmotor"  ).Apply( vel, vel );
        Bot.GetComponent<Servomotor>(  "rmotor"  ).Apply( vel, vel );
        Bot.GetComponent<Servomotor>(  "blmotor"  ).Apply( vel, vel );
        Bot.GetComponent<Servomotor>(  "brmotor"  ).Apply( vel, vel );

        while( vrfVerdeRight() ) {
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
        while( vrfVerdeLeft() ) {
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
