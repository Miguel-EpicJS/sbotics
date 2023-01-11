bool VrfDireita() {
    return ( ( ( ( Bot.GetComponent<ColorSensor>( "rc" ).Analog ).ToString() == "Preto" ||  ( Bot.GetComponent<ColorSensor>( "rc2" ).Analog ).ToString() == "Preto" ) && ( Bot.GetComponent<ColorSensor>( "lc" ).Analog ).ToString() != "Preto" ) );
}

bool VrfEsquerda() {
    return ( ( ( ( Bot.GetComponent<ColorSensor>( "lc" ).Analog ).ToString() == "Preto" || ( Bot.GetComponent<ColorSensor>( "lc2" ).Analog ).ToString() == "Preto" ) && ( Bot.GetComponent<ColorSensor>( "rc" ).Analog ).ToString() != "Preto" ) ) ;
}

void Direcao(string direction = "d", double mult = 1) {
    if(direction == "d") {
        Bot.GetComponent<Servomotor>("lmotor").Apply( 170*mult, 170*mult );
        Bot.GetComponent<Servomotor>("rmotor").Apply( -150*mult, -150*mult );
        Bot.GetComponent<Servomotor>("blmotor").Apply( 170*mult, 170*mult );
        Bot.GetComponent<Servomotor>("brmotor").Apply( -150*mult, -150*mult );
    } else {
        Bot.GetComponent<Servomotor>("lmotor").Apply( -150*mult, -150*mult );
        Bot.GetComponent<Servomotor>("rmotor").Apply( 170*mult, 170*mult  );
        Bot.GetComponent<Servomotor>("blmotor").Apply( -150*mult, -150*mult );
        Bot.GetComponent<Servomotor>("brmotor").Apply( 170*mult, 170*mult  );
    }
}

void Travar() {
    Bot.GetComponent<Servomotor>(  "lmotor"  ).Locked = true;
    Bot.GetComponent<Servomotor>(  "rmotor"  ).Locked = true;
    Bot.GetComponent<Servomotor>(  "blmotor"  ).Locked = true; 
    Bot.GetComponent<Servomotor>(  "brmotor"  ).Locked = true;
}

async Task Direita(double mult, int type = 0) {

    Direcao("d", mult);

    if(type == 1) { // Direita 90
        double actCompass = Bot.Compass;
        while(Math.Round(Bot.Compass) != Utils.Modulo(Math.Round(actCompass+90), 360)) {
            await Time.Delay(50);
            IO.Print( Math.Round(Bot.Compass).ToString()+ "  " + Utils.Modulo(Math.Round(actCompass+90), 360).ToString());
            Direita(mult);
        }
    }

}

async Task Esquerda(double mult, int type = 0, int modf = 0) {

    Direcao("e", mult);

    if(type == 1) { // Esquerda 90
        double actCompass = Bot.Compass - 90 + modf;

        while(Math.Round(Bot.Compass) != Utils.Modulo(Math.Round(actCompass), 360)) {
            await Time.Delay(50);
            IO.Print( Math.Round(Bot.Compass).ToString()+ "  " +  Utils.Modulo(Math.Round(actCompass), 360).ToString());
            Esquerda(mult);
        }
    }

}

void Destravar() {
    Bot.GetComponent<Servomotor>(  "lmotor"  ).Locked = false;
    Bot.GetComponent<Servomotor>(  "rmotor"  ).Locked = false;
    Bot.GetComponent<Servomotor>(  "blmotor"  ).Locked = false; 
    Bot.GetComponent<Servomotor>(  "brmotor"  ).Locked = false;
}

void Frente(double vel) {
        Bot.GetComponent<Servomotor>(  "lmotor"  ).Apply( vel, vel );
        Bot.GetComponent<Servomotor>(  "rmotor"  ).Apply( vel, vel );
        Bot.GetComponent<Servomotor>(  "blmotor"  ).Apply( vel, vel );
        Bot.GetComponent<Servomotor>(  "brmotor"  ).Apply( vel, vel );
}

async Task Main()
{
    Destravar();
    Levantar(5);
    IO.Print("Exit");
    double vel = 180;
    double rvel = 3;

    while(true) {
        await Time.Delay(50);

        IO.Print($"{(Bot.Speed).ToString()} {vel}");
        while(Bot.Speed < 1.5) {
             await Time.Delay(50);
             vel += 5;
        } 
        while(Bot.Speed > 1.5) {
             await Time.Delay(50);
             vel -= 5;
        }

        Frente(vel);
        while( VrfDireita() ) {
            await Time.Delay(50);

            await Direita(rvel);

        }
        while( VrfEsquerda() ) {
            await Time.Delay(50);

            await Esquerda(rvel);

        }
        
    }


}
