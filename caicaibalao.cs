async Task Main()
{
    // 33x
    /*Bot.GetComponent<Buzzer>( "buzzer" ).PlaySound( 264 ); // do
    Bot.GetComponent<Buzzer>( "buzzer" ).PlaySound( 297 ); // re 
    Bot.GetComponent<Buzzer>( "buzzer" ).PlaySound( 330 ); // mi
    Bot.GetComponent<Buzzer>( "buzzer" ).PlaySound( 352 ); // fa 
    Bot.GetComponent<Buzzer>( "buzzer" ).PlaySound( 396 ); // sol
    Bot.GetComponent<Buzzer>( "buzzer" ).PlaySound( 440 ); // la
    Bot.GetComponent<Buzzer>( "buzzer" ).PlaySound( 495 ); // si
    Bot.GetComponent<Buzzer>( "buzzer" ).PlaySound( 528 ); // do */

    Bot.GetComponent<Buzzer>( "buzzer" ).PlaySound( 396*2 ); // sol
    await Time.Delay(200);
    Bot.GetComponent<Buzzer>( "buzzer" ).StopSound();
    await Time.Delay(100);
    Bot.GetComponent<Buzzer>( "buzzer" ).PlaySound( 396*2 ); // sol
    await Time.Delay(200);
    Bot.GetComponent<Buzzer>( "buzzer" ).StopSound();
    await Time.Delay(100);
    Bot.GetComponent<Buzzer>( "buzzer" ).PlaySound( 352*2 ); // fa 
    await Time.Delay(200);
    Bot.GetComponent<Buzzer>( "buzzer" ).StopSound();
    await Time.Delay(100);
    Bot.GetComponent<Buzzer>( "buzzer" ).PlaySound( 330*2 ); // mi
    await Time.Delay(200);
    Bot.GetComponent<Buzzer>( "buzzer" ).StopSound();
    await Time.Delay(100);

    Bot.GetComponent<Buzzer>( "buzzer" ).PlaySound( 396*2 ); // sol
    await Time.Delay(200);
    Bot.GetComponent<Buzzer>( "buzzer" ).StopSound();
    await Time.Delay(100);
    Bot.GetComponent<Buzzer>( "buzzer" ).PlaySound( 440*2 ); // la
    await Time.Delay(200);
    Bot.GetComponent<Buzzer>( "buzzer" ).StopSound();
    await Time.Delay(100);
    Bot.GetComponent<Buzzer>( "buzzer" ).PlaySound( 396*2 ); // sol
    await Time.Delay(200);
    Bot.GetComponent<Buzzer>( "buzzer" ).StopSound();
    await Time.Delay(100);
    Bot.GetComponent<Buzzer>( "buzzer" ).PlaySound( 352*2 ); // fa 
    await Time.Delay(200);
    Bot.GetComponent<Buzzer>( "buzzer" ).StopSound();
    await Time.Delay(100);
    Bot.GetComponent<Buzzer>( "buzzer" ).PlaySound( 297*2 ); // re 
    await Time.Delay(200);
    Bot.GetComponent<Buzzer>( "buzzer" ).StopSound();
    await Time.Delay(100);

    for(int i =0; i < 3; i++) {
        Bot.GetComponent<Buzzer>( "buzzer" ).PlaySound( 297*2 ); // re 
        await Time.Delay(200);
        Bot.GetComponent<Buzzer>( "buzzer" ).StopSound();
        await Time.Delay(100);
        Bot.GetComponent<Buzzer>( "buzzer" ).PlaySound( 330*2 ); // mi
        await Time.Delay(200);
        Bot.GetComponent<Buzzer>( "buzzer" ).StopSound();
        await Time.Delay(100);
        Bot.GetComponent<Buzzer>( "buzzer" ).PlaySound( 352*2 ); // fa 
        await Time.Delay(200);
        Bot.GetComponent<Buzzer>( "buzzer" ).StopSound();
        await Time.Delay(100);
    }

    Bot.GetComponent<Buzzer>( "buzzer" ).PlaySound( 396*2 ); // sol
    await Time.Delay(200);
    Bot.GetComponent<Buzzer>( "buzzer" ).StopSound();
    await Time.Delay(100);
    Bot.GetComponent<Buzzer>( "buzzer" ).PlaySound( 440*2 ); // la
    await Time.Delay(200);
    Bot.GetComponent<Buzzer>( "buzzer" ).StopSound();
    await Time.Delay(100);
    Bot.GetComponent<Buzzer>( "buzzer" ).PlaySound( 396*2 ); // sol
    await Time.Delay(200);
    Bot.GetComponent<Buzzer>( "buzzer" ).StopSound();
    await Time.Delay(100);
    Bot.GetComponent<Buzzer>( "buzzer" ).PlaySound( 352*2 ); // fa 
    await Time.Delay(200);
    Bot.GetComponent<Buzzer>( "buzzer" ).StopSound();
    await Time.Delay(100);
    Bot.GetComponent<Buzzer>( "buzzer" ).PlaySound( 330*2 ); // mi
    await Time.Delay(200);
    Bot.GetComponent<Buzzer>( "buzzer" ).StopSound();
    await Time.Delay(100);
    Bot.GetComponent<Buzzer>( "buzzer" ).PlaySound( 297*2 ); // re 
    await Time.Delay(200);
    Bot.GetComponent<Buzzer>( "buzzer" ).StopSound();
    await Time.Delay(100);
    Bot.GetComponent<Buzzer>( "buzzer" ).PlaySound( 264*2 ); // do
    await Time.Delay(200);
}
