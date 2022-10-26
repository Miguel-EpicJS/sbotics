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

async Task FuncPa(double mult, int dir) {

   Bot.GetComponent<Servomotor>( "fmotor" ).Locked = false;
   Bot.GetComponent<Servomotor>("fmotor").Apply( 50*mult*dir, 50*mult*dir );
   await Time.Delay(50*mult);
   Bot.GetComponent<Servomotor>( "fmotor" ).Locked = true;
}

async Task DirecaoPa(double mult = 1, int dir = 1) {
    Bot.GetComponent<Servomotor>( "fmotor" ).Locked = false;

    int angle = (dir == 1 ? 40 : 0);
    if(angle == 40) {
        while(( Bot.GetComponent<Servomotor>( "fmotor" ).Angle ) < angle) {
            IO.Print("Pa1");
            await Time.Delay(50);
            await FuncPa(mult, dir);
        }
    } else {
        while(( Bot.GetComponent<Servomotor>( "fmotor" ).Angle ) > angle) {
            IO.Print("Pa1");
            await Time.Delay(50);
            await FuncPa(mult, dir);
        }
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
        Bot.GetComponent<Servomotor>("fmotor").Apply( 45*mult, 45*mult );
        Frente(motores, 3);
        await Time.Delay(120);
        Travar(motores, true);
    }
    Travar(motores, false);
    Bot.GetComponent<Servomotor>( "fmotor" ).Locked = true;
    IO.Print("Fim2");
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

bool VerificarUltra(string nome, double limite, bool menor = true) {

    if(menor) {
        return ( Bot.GetComponent<UltrasonicSensor>( nome ).Analog ) < limite && ( Bot.GetComponent<UltrasonicSensor>( nome ).Analog ) > 0;
    } else {
        return ( Bot.GetComponent<UltrasonicSensor>( nome ).Analog ) > limite && ( Bot.GetComponent<UltrasonicSensor>( nome ).Analog ) != -1;
    }

}

async Task Start() {
    Travar(motores, false);
    Frente(motores, 1);
    await DirecaoPa(3, 1);
}

async Task Stage1() {
    while(true) {

        await Time.Delay(50);
        Frente(motores, 1);
        if( ( Bot.GetComponent<ColorSensor>( "lc1" ).Analog ).Brightness < 90 && (Bot.GetComponent<ColorSensor>( "rc1" ).Analog ).Brightness < 90) {
            await Time.Delay(200);
            break;
        }
    }
}

async Task Stage2() {

    await DirecaoPa(1, -1);


    while(true) {
        await Time.Delay(50);
        Frente(motores, 2);
        if( ( Bot.GetComponent<ColorSensor>( "fs" ).Analog ).ToString() == "Preto") {
            await Time.Delay(2000);
            break;
        }
        else if(VerificarUltra("ffultra", 6)) {
            await Direita(1.2, 1);
        }
        
    }

}

async Task Stage3() {
    while(true) {
        await Time.Delay(50);
        Tras(motores, 1);
        if(VerificarUltra("bultra", 1)) {
            Travar(motores);
        }
        if(VerificarUltra("rultra", 18)) {

            Tras(motores, 1.5);

            await Time.Delay(1000);
            await Direita(1.2, 1);

            while(VerificarUltra("ffultra", 5, false)) {
                await Time.Delay(50);
                Frente(motores, 2);
            }
           
            await LevantarPa2(40, 0.1);
            await Time.Delay(4000);

            while(!(VerificarUltra("bultra", 1))) {
                await Time.Delay(50);
                Tras(motores, 1);
            }
            await Esquerda(1.2, 1, 20);
            
            await DirecaoPa(1, -1);
            while( ( Bot.GetComponent<ColorSensor>( "fs" ).Analog ).ToString() != "Preto") {
                await Time.Delay(50);
                Frente(motores, 2);
            }
            await Time.Delay(2000);
        }

    }
}

async Task Main()
{

    
    await Start();
    

    await Stage1();

    await Stage2();

    await Stage3();

}
