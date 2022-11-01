async Task LuzP(string nome, double[] cor) {
    Bot.GetComponent<Light>( nome ).TurnOn( new Color(cor[0], cor[1], cor[2]) );
    await Time.Delay(100);
    Bot.GetComponent<Light>( nome ).TurnOff();
}

void Luz(string nome, double[] cor ) {
    Bot.GetComponent<Light>( nome ).TurnOn( new Color(cor[0], cor[1], cor[2]) );
}

void LuzO(string nome, double[] cor ) {
    Bot.GetComponent<Light>( nome ).TurnOff();
}

double[] yellow = { 255, 255, 0};

double[] green = { 0, 100, 0};

double[] red = { 255, 0, 0};

double[] white = { 255, 255, 255};

async Task Main()
{
    Bot.GetComponent<Servomotor>( "m1" ).Locked = false;
    Bot.GetComponent<Servomotor>( "m2" ).Locked = false;
    Bot.GetComponent<Servomotor>( "m3" ).Locked = false;
    Bot.GetComponent<Servomotor>( "m4" ).Locked = false;

    
    Bot.GetComponent<Servomotor>( "m1" ).Apply( 10 , -500 );
    Bot.GetComponent<Servomotor>( "m4" ).Apply( 10 , -500 );
    Bot.GetComponent<Servomotor>( "m3" ).Apply( 10 , 500 );
    Bot.GetComponent<Servomotor>( "m2" ).Apply( 10 , 500 );

    await Time.Delay(1000);

    Bot.GetComponent<Servomotor>( "m1" ).Apply( 10 , -500 );
    Bot.GetComponent<Servomotor>( "m2" ).Apply( 10 , -500 );
    Bot.GetComponent<Servomotor>( "m3" ).Apply( 10 , -500 );
    Bot.GetComponent<Servomotor>( "m4" ).Apply( 10 , -500 );

    int[] leds = { 12, 36 };
    for(int i = 1; i <= 12; i++ ) {
        Luz($"bl{i}", green );
    }
    while(true) {
        for(int j = 1; j <= 5; j++) {
            await Time.Delay(50);
            for(int i = 1; i <= 36; i++) {
                await Time.Delay(50);
                LuzP($"f{(i+0)%36}", green);
                LuzP($"f{(i+1)%36}", white);
                LuzP($"f{(i+2)%36}", red);

                LuzP($"f{(i+5)%36}", green);
                LuzP($"f{(i+6)%36}", white);
                LuzP($"f{(i+7)%36}", red);
            }
        }

            Bot.GetComponent<Servomotor>( "m1" ).Apply( 500 , -500 );
            Bot.GetComponent<Servomotor>( "m4" ).Apply( 500 , -500 );
            Bot.GetComponent<Servomotor>( "m3" ).Apply( 500 , 500 );
            Bot.GetComponent<Servomotor>( "m2" ).Apply( 500 , 500 );

            await Time.Delay(3000);

    
            Bot.GetComponent<Servomotor>( "m1" ).Apply( 10 , -500 );   
            Bot.GetComponent<Servomotor>( "m2" ).Apply( 10 , -500 );
            Bot.GetComponent<Servomotor>( "m3" ).Apply( 10 , -500 );
            Bot.GetComponent<Servomotor>( "m4" ).Apply( 10 , -500 );
        for(int j = 1; j <= 5; j++) {
            await Time.Delay(200);
            for(int i = 1; i <= 36; i+=3) {
                Luz($"f{(i+0)%36}", green);
                Luz($"f{(i+1)%36}", white);
                Luz($"f{(i+2)%36}", red);
            }
            

            await Time.Delay(200);
            for(int i = 1; i <= 36; i+=3) {
                LuzO($"f{(i+0)%36}", green);
                LuzO($"f{(i+1)%36}", white);
                LuzO($"f{(i+2)%36}", red);
            }

        }
    }
}
