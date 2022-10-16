string[] motores = {"lmotor", "rmotor", "blmotor", "brmotor", "fmotor"};


void Travar(string[] nomes, bool val = true) {
    foreach(string nome in nomes) {
        Bot.GetComponent<Servomotor>(nome).Locked = val;
    }
}

async Task Parar(string[] nomes) {
    foreach(string nome in nomes) {
        Bot.GetComponent<Servomotor>(nome).Apply( 0, 0 );
    }
}

async Task LevantarPa() {
    Bot.GetComponent<Servomotor>( "fmotor" ).Locked = false;
    while(( Bot.GetComponent<Servomotor>( "fmotor" ).Angle ) < 40) {
        IO.Print("Pa1");
        await Time.Delay(50);
        Bot.GetComponent<Servomotor>( "fmotor" ).Locked = false;
        Bot.GetComponent<Servomotor>("fmotor").Apply( 150, 150 );
        await Time.Delay(130);
        Bot.GetComponent<Servomotor>( "fmotor" ).Locked = true;
    }
    Bot.GetComponent<Servomotor>( "fmotor" ).Locked = true;
    IO.Print("Fim1");
}

async Task LevantarPa2(double angle = 31, double mult = 1) {
    Travar(motores, false);
    while(( Bot.GetComponent<Servomotor>( "fmotor" ).Angle ) < angle) {
        IO.Print($"{( Bot.Speed )}");
        await Time.Delay(50);
        Travar(motores, false);
        Bot.GetComponent<Servomotor>( "fmotor" ).Locked = true;
        Tras(motores, 1.5);
        await Time.Delay(100);

        
        Bot.GetComponent<Servomotor>( "fmotor" ).Locked = false;
        Bot.GetComponent<Servomotor>("fmotor").Apply( 50*mult, 50*mult );
        Frente(motores, 3);
        await Time.Delay(130);
        Travar(motores, true);
    }
    Travar(motores, false);
    Bot.GetComponent<Servomotor>( "fmotor" ).Locked = true;
    IO.Print("Fim2");
}


async Task AbaixarPa(double mult = 1) {
    while(( Bot.GetComponent<Servomotor>( "fmotor" ).Angle ) >= 0) {
        await Time.Delay(50);
        Bot.GetComponent<Servomotor>( "fmotor" ).Locked = false;
        Bot.GetComponent<Servomotor>("fmotor").Apply( -100*mult, -100*mult );
        await Time.Delay(50);
        Bot.GetComponent<Servomotor>( "fmotor" ).Locked = true;
    }
}

async Task Frente(string[] nomes, double mult = 1) {
    foreach(string nome in nomes) {
        Bot.GetComponent<Servomotor>(nome).Apply( 150*mult, 150*mult );
    }
}

async Task Tras(string[] nomes, double mult) {
    foreach(string nome in nomes) {
        Bot.GetComponent<Servomotor>(nome).Apply( -150*mult, -150*mult );
    }
}

async Task Direita(double mult) {

    Bot.GetComponent<Servomotor>("lmotor").Apply( 170*mult, 170*mult );
    Bot.GetComponent<Servomotor>("rmotor").Apply( -150*mult, -150*mult );
    Bot.GetComponent<Servomotor>("blmotor").Apply( 170*mult, 170*mult );
    Bot.GetComponent<Servomotor>("brmotor").Apply( -150*mult, -150*mult );

}

async Task Direita90(double mult = 1, double actCompass = 270) {
    while(Math.Round(Bot.Compass) != Utils.Modulo(Math.Round(actCompass+90), 360)) {
        await Time.Delay(50);
        IO.Print( Math.Round(Bot.Compass).ToString()+ "  " + Utils.Modulo(Math.Round(actCompass+90), 360).ToString());
        Direita(mult);
   }
}

async Task DireitaGrau(double mult = 1, double actCompass = 270, double grau = 90) {
    while(Math.Round(Bot.Compass) != Utils.Modulo(Math.Round(actCompass+grau), 360)) {
        await Time.Delay(50);
        IO.Print( Math.Round(Bot.Compass).ToString()+ "  " + Utils.Modulo(Math.Round(actCompass+grau), 360).ToString());
        Direita(mult);
   }
}

async Task Esquerda90(double mult = 1, double actCompass = 270) {
    actCompass -= 90;
    while(Math.Round(Bot.Compass) != Utils.Modulo(Math.Round(actCompass), 360)) {
        await Time.Delay(50);
        IO.Print( Math.Round(Bot.Compass).ToString()+ "  " +  Utils.Modulo(Math.Round(actCompass), 360).ToString());
        Esquerda(mult);
   }
}

async Task EsquerdaGrau(double mult = 1, double actCompass = 270, double grau = 90) {
    actCompass -= grau;
    while(Math.Round(Bot.Compass) != Utils.Modulo(Math.Round(actCompass), 360)) {
        await Time.Delay(50);
        IO.Print( Math.Round(Bot.Compass).ToString()+ "  " +  Utils.Modulo(Math.Round(actCompass), 360).ToString());
        Esquerda(mult);
   }
}

async Task Esquerda(double mult) {

    Bot.GetComponent<Servomotor>("lmotor").Apply( -150*mult, -150*mult );
    Bot.GetComponent<Servomotor>("rmotor").Apply( 170*mult, 170*mult  );
    Bot.GetComponent<Servomotor>("blmotor").Apply( -150*mult, -150*mult );
    Bot.GetComponent<Servomotor>("brmotor").Apply( 170*mult, 170*mult  );

}


async Task Main()
{

    Travar(motores, false);
    await LevantarPa();

    while(true) {

        await Time.Delay(50);
        Frente(motores, 2);
        if( ( Bot.GetComponent<ColorSensor>( "lc1" ).Analog ).Brightness < 90 && ( Bot.GetComponent<ColorSensor>( "rc1" ).Analog ).Brightness < 90) {
            break;
        }
    }
    await AbaixarPa();


    double anguloT = 0;

    while(true) {
        await Time.Delay(50);
        Frente(motores, 2);
        if( ( Bot.GetComponent<ColorSensor>( "fs" ).Analog ).ToString() == "Preto") {
            anguloT = Math.Round(Bot.Compass);
            IO.Print(anguloT.ToString());
            await Time.Delay(500);
            break;
        }
        else if(( Bot.GetComponent<UltrasonicSensor>( "ffultra" ).Analog ) < 6 && ( Bot.GetComponent<UltrasonicSensor>( "ffultra" ).Analog ) > 0) {
            
            await Direita90(1.2, Bot.Compass);
        }
        
    }

    while(true) {
        await Time.Delay(50);
        Tras(motores, 2);
        if((( Bot.GetComponent<UltrasonicSensor>( "bultra" ).Analog ) < 1 && ( Bot.GetComponent<UltrasonicSensor>( "bultra" ).Analog ) > 0)) {
            Travar(motores);
        }
        if(( Bot.GetComponent<UltrasonicSensor>( "rultra" ).Analog ) < 18 && ( Bot.GetComponent<UltrasonicSensor>( "rultra" ).Analog ) > 0) {

            Tras(motores, 2);

            await Time.Delay(500);
            await Direita90(1.2, Bot.Compass);

            while(( Bot.GetComponent<UltrasonicSensor>( "ffultra" ).Analog ) > 5  && ( Bot.GetComponent<UltrasonicSensor>( "ffultra" ).Analog ) != -1) {
                await Time.Delay(50);
                Frente(motores, 2);
            }
           
            await LevantarPa2(40, 0.1);
            await Time.Delay(4000);

            while(!(( Bot.GetComponent<UltrasonicSensor>( "bultra" ).Analog ) < 1 && ( Bot.GetComponent<UltrasonicSensor>( "bultra" ).Analog ) > 0)) {
                await Time.Delay(50);
                Tras(motores, 2);
            }
            await Esquerda90(1.2, Bot.Compass + 10);
          
            Travar(motores);
            Travar(motores, false);
            
            await AbaixarPa();
            while( ( Bot.GetComponent<ColorSensor>( "fs" ).Analog ).ToString() != "Preto") {
                await Time.Delay(50);
                Frente(motores, 2);
            }
        }

    }

}
