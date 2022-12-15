void Locked(bool option)
{
    Bot.GetComponent<Servomotor>(  "lmotor"  ).Locked = option;
    Bot.GetComponent<Servomotor>(  "rmotor"  ).Locked = option;
    Bot.GetComponent<Servomotor>(  "blmotor"  ).Locked = option; 
    Bot.GetComponent<Servomotor>(  "brmotor"  ).Locked = option;
}

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

async Task Main()
{
    Locked(false);
 
    await Left(150, 45);

    Locked(true);
}
