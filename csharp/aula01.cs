async Task Main()
{
    // Destravando motores
    Bot.GetComponent<Servomotor>(  "lmotor"  ).Locked = false;
    Bot.GetComponent<Servomotor>(  "rmotor"  ).Locked = false;
    Bot.GetComponent<Servomotor>(  "blmotor"  ).Locked = false; 
    Bot.GetComponent<Servomotor>(  "brmotor"  ).Locked = false;

    // guardando valores
    int vel = 140;
    int rvel = 300;
    bool turn = true;

    while(true) {
        await Time.Delay(50);

        // Movimentando para frente
        Bot.GetComponent<Servomotor>(  "lmotor"  ).Apply( vel, vel );
        Bot.GetComponent<Servomotor>(  "rmotor"  ).Apply( vel, vel );
        Bot.GetComponent<Servomotor>(  "blmotor"  ).Apply( vel, vel );
        Bot.GetComponent<Servomotor>(  "brmotor"  ).Apply( vel, vel );

        // verificar por inclinação        
        if ( (Bot.Inclination + 10) % 360 > 300) {
            vel = 500;
            turn = false;
        } else {
            vel = 140;
            turn = true;
        }

        // virar para a direita
        while( ( Bot.GetComponent<ColorSensor>( "rc1" ).Analog ).ToString() == "Preto" && turn) {
            await Time.Delay(50);
            Bot.GetComponent<Servomotor>(  "lmotor"  ).Apply( rvel, rvel );
            Bot.GetComponent<Servomotor>(  "rmotor"  ).Apply( -rvel, -rvel);
            Bot.GetComponent<Servomotor>(  "blmotor"  ).Apply( rvel, rvel );
            Bot.GetComponent<Servomotor>(  "brmotor"  ).Apply( -rvel, -rvel);
        }

        // virar para a esquerda
        while( ( Bot.GetComponent<ColorSensor>( "lc1" ).Analog ).ToString() == "Preto" && turn) {
            await Time.Delay(50);
            Bot.GetComponent<Servomotor>(  "lmotor"  ).Apply( -rvel, -rvel );
            Bot.GetComponent<Servomotor>(  "rmotor"  ).Apply( rvel, rvel );
            Bot.GetComponent<Servomotor>(  "blmotor"  ).Apply( -rvel, -rvel );
            Bot.GetComponent<Servomotor>(  "brmotor"  ).Apply( rvel, rvel );
        }
    }
}
