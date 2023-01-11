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

        while( ( Bot.GetComponent<ColorSensor>( "rc" ).Analog ).ToString() == "Preto" ) {
            await Time.Delay(50);
            Bot.GetComponent<Servomotor>(  "lmotor"  ).Apply( rvel, rvel );
            Bot.GetComponent<Servomotor>(  "rmotor"  ).Apply( rvel, rvel - 2*rvel );
            Bot.GetComponent<Servomotor>(  "blmotor"  ).Apply( rvel, rvel );
            Bot.GetComponent<Servomotor>(  "brmotor"  ).Apply( rvel, rvel - 2*rvel );

        }
        while( ( Bot.GetComponent<ColorSensor>( "lc" ).Analog ).ToString() == "Preto" ) {
            await Time.Delay(50);
            Bot.GetComponent<Servomotor>(  "lmotor"  ).Apply( rvel, rvel - 2*rvel );
            Bot.GetComponent<Servomotor>(  "rmotor"  ).Apply( rvel, rvel );
            Bot.GetComponent<Servomotor>(  "blmotor"  ).Apply( rvel, rvel - 2*rvel );
            Bot.GetComponent<Servomotor>(  "brmotor"  ).Apply( rvel, rvel );

        }
        
    }


}
