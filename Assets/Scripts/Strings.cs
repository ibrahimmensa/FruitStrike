using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strings 
{
    public static string infoText;
	public static string welcome;
	public static string next;
	public static string tap;
	public static string skip;
	public static string tutorial0;
	public static string tutorial1;
	public static string tutorial2;
	public static string tutorial3;
	public static string tutorial4;
	public static string tutorial5;
	public static string tutorial5b;
	public static string tutorial6;
	public static string tutorial6b;
	public static string tutorial7;
	public static string tutorial7b;
	public static string tutorial8;
	public static string tutorial9;
	public static string tutorial9b;
	public static string invite1;
	public static string invite2;
	public static string invite3;
	public static string withdrawal1;
	public static string withdrawal2;
	public static string withdrawal3;
	public static string withdrawal4;
	public static string play;
	public static string leaderboard;
	public static string exit;
	public static string leaderboard1;
	public static string leaderboard2;
	public static string leaderboard3;
	public static string leaderboard4;
	public static string leaderboard5;
	public static string earn;
	public static string bidText;
	public static string bidText2;
	public static string surveyText;
	
	public static void setLanguage(){
		
		float inviteReward = NetworkScript.inviteReward * 2;
		if(inviteReward == 0)
			inviteReward = 600;
		
		switch(FruitUI.currency){
			
			case "english":
				
				play = "Play";
				leaderboard = "Leaderboard";
				exit = "Exit";
				earn = "EARN MORE!";
				infoText = "Players that collect the most green apples are rewarded daily!\n\nEnsure your internet data is on when playing in order to record your score.\n\nWinners are rewarded at the beginning of the next day by 12:00AM (GMT) and the leaderboard is reset for the new day.\n\nGood Luck!";
				welcome = "Welcome";
				next = "Next >>>";
				skip = "Skip >>";
				tap = "(Tap to Continue)";
				tutorial0 = "Let me put you through a quick tutorial of how this works...";
				tutorial1 = "Play Game";
				tutorial2 = "Shoot and collect Cash Tokens";
				tutorial3 = "Collect Apples";
				tutorial4 = "Use Red Apples to Compete in Daily Tournaments and Win Money";
				tutorial5 = "Players who collect the most green apples win money daily!";
				tutorial5b = "(You can view Daily Stats on the Leaderboard)";
				tutorial6 = "Once you reach payment threshold you can cash out via PayPal/Bank Account";
				tutorial6b = "(Withdrawal is processed within 2-8 working days)";
				tutorial7 = "Invite your friends and receive ca$h when they sign up";
				tutorial7b = "(You get paid when somone logs in using facebook and puts your referral code)";
				tutorial8 = "You Can Always Watch this Tutorial Again Any time you want";
				tutorial9 = "Okay, I think that's all you need to know...";
				tutorial9b = "Log In to begin";
				invite1 = "Get "+inviteReward+" cash tokens for every 2 friends you invite !!!";
				invite2 = "1. Invites must sign up using facebook\n2. The more active your invites, the more your invite reward (and vice-versa)";
				invite3 = " more friends to get cash reward";
				withdrawal1 = "Convert your cash tokens to real money";
				withdrawal2 = "(Withdrawal is processed within 2-8 business days)";
				withdrawal3 = "Enter your Paypal email";
				withdrawal4 = "Payments will be made on the ";
				surveyText = "Read and answer the survey questions carefully so you can get your cash tokens reward.\nRushing through will get you disqualified.";
			
				leaderboard1 = "Today's Leaderboard";
				leaderboard2 = "Today's Highscores";
				leaderboard3 = "Top Highscores";
				leaderboard4 = "Top Earners";
				leaderboard5 = "Yesterday's Leaderboard";
				
				bidText = "3 Winners selected at random from the top bidders";
				bidText2 = "Bid More Apples for Higher Chance of Winning!";
				
				string[] tips = {"Stay connected to receive free apples during gameplay",
                                "Invite your freinds and earn cash rewards",
                                "Stuck on a stage?, click on the 'skip' button at the top right",
                                "Feeling lucky? Take a spin at the wheel of fortune and Win Prizes!",
                                 "Green Apples are more frequent as your stage increases"};
				
				FruitUI.tips = tips;
	
			
			break;
			
			case "french":
			
				play = "Jouer";
				leaderboard = "Leaderboard";
				exit = "sortir";
				earn = "GAGNE PLUS!";
				infoText = "Les joueurs qui récoltent le plus de pommes vertes sont récompensés quotidiennement! \n\nAssurez-vous que vos données Internet sont activées lorsque vous jouez afin d'enregistrer votre score. \n\nLes gagnants sont récompensés au début du jour suivant à minuit (GMT) et le classement est réinitialisé pour le nouveau jour.\n\nBonne chance!";
				welcome = "Bienvenue";
				next = "Suivant >>>";
				skip = "Sauter >>";
				tap = "(Appuyez pour continuer)";
				tutorial0 = "Permettez-moi de vous expliquer rapidement comment cela fonctionne ...";
				tutorial1 = "Jouer au jeu";
				tutorial2 = "Tirez et collectez des jetons d'argent";
				tutorial3 = "Ramassez des pommes";
				tutorial4 = "Utilisez des pommes rouges pour participer à des tournois quotidiens et gagner de l'argent";
				tutorial5 = "Les joueurs qui récoltent le plus de pommes vertes gagnent de l'argent quotidiennement!";
				tutorial5b = "(Vous pouvez afficher les statistiques quotidiennes sur le Leaderboard)";
				tutorial6 = "Une fois que vous avez atteint le seuil de paiement, vous pouvez retirer via PayPal / compte bancaire";
				tutorial6b = "(Le retrait est traité dans les 2-8 jours ouvrables)";
				tutorial7 = "Invitez vos amis et recevez de l'argent lors de leur inscription";
				tutorial7b = "(Vous êtes payé lorsque quelqu'un se connecte en utilisant Facebook et met votre code de parrainage)";
				tutorial8 = "Vous pouvez toujours regarder à nouveau ce didacticiel à tout moment";
				tutorial9 = "D'accord, je pense que c'est tout ce que vous devez savoir ...";
				tutorial9b = "Connectez-vous pour commencer";
				invite1 = "Obtenez "+inviteReward+" cash pour 2 amis que vous invitez !!!";
				invite2 = "1. Les invités doivent s'inscrire via facebook \n2. Plus vos invitations sont actives, plus votre récompense d'invitation (et vice versa) :)";
				invite3 = " plus d'amis pour obtenir une récompense en espèces";
				withdrawal1 = "Convertissez vos jetons en argent réel";
				withdrawal2 = "(Le retrait est traité dans un délai de 2 à 8 jours ouvrables)";
				withdrawal3 = "Entrez votre email Paypal";
				withdrawal4 = "Les paiements seront effectués le ";
				surveyText = "Lisez et répondez attentivement aux questions de l'enquête afin d'obtenir votre récompense en espèces.\nEn vous précipitant, vous serez disqualifié.";
			
				bidText = "3 gagnants sélectionnés au hasard parmi les meilleurs enchérisseurs";
				bidText2 = "Offrez plus de pommes pour plus de chances de gagner!";
				
				string[] tips2 = {"Restez connecté pour recevoir des pommes gratuites pendant le jeu",
                                "Invitez vos amis et gagnez des récompenses en espèces",
                                "Coincé sur une scène?, Cliquez sur le bouton 'sauter' en haut à droite",
                                "Vous vous sentez chanceux? Faites un tour à la roue de la fortune et gagnez des prix!",
                                 "Les pommes vertes sont plus fréquentes à mesure que votre niveau augmente"};
				
				FruitUI.tips = tips2;
				
			
			break;
			
			case "portugese":
			
				play = "Toque";
				leaderboard = "Leaderboard";
				exit = "Saída";
				earn = "GANHAR MAIS!";
				infoText = "Os jogadores que coletarem mais maçãs verdes são recompensados ​​diariamente! \n\nCertifique-se de que seus dados de Internet estejam ligados ao jogar para registrar sua pontuação. \n\nOs vencedores são recompensados ​​no início do dia seguinte às 12:00 (GMT) e a tabela de classificação é redefinida para o novo dia. \n\nBoa sorte!";
				welcome = "Receber";
				next = "Próximo >>>";
				skip = "Pular >>";
				tap = "(Clique para continuar)";
				tutorial0 = "Deixe-me mostrar um rápido tutorial de como isso funciona ...";
				tutorial1 = "Jogar um jogo";
				tutorial2 = "Atire e colete fichas de dinheiro";
				tutorial3 = "Colete Maçãs";
				tutorial4 = "Use Red Apples para competir em torneios diários e ganhar dinheiro";
				tutorial5 = "Os jogadores que coletarem mais maçãs verdes ganham dinheiro diariamente!";
				tutorial5b = "(Você pode ver as estatísticas diárias no Leaderboard)";
				tutorial6 = "Depois de atingir o limite de pagamento, você pode sacar via PayPal / conta bancária";
				tutorial6b = "(A retirada é processada dentro de 2-8 dias úteis)";
				tutorial7 = "Convide seus amigos e receba dinheiro quando eles se inscreverem";
				tutorial7b = "(Você é pago quando alguém faz login usando o Facebook e coloca seu código de referência)";
				tutorial8 = "Você pode sempre assistir a este tutorial novamente, sempre que quiser";
				tutorial9 = "Ok, acho que é tudo que você precisa saber ...";
				tutorial9b = "Faça login para começar";
				invite1 = "Ganhe "+inviteReward+" em dinheiro para cada 2 amigos que você convidar !!!";
				invite2 = "1. Os convites devem se inscrever usando o Facebook \n2. Quanto mais ativos forem seus convites, maior será sua recompensa de convite (e vice-versa)";
				invite3 = " mais amigos para obter recompensa em dinheiro";
				withdrawal1 = "Converta seus tokens de dinheiro em dinheiro real";
				withdrawal2 = "(A retirada é processada dentro de 2-8 dias úteis)";
				withdrawal3 = "Digite seu e-mail do Paypal";
				withdrawal4 = "Os pagamentos serão feitos no ";
				surveyText = "Leia e responda às perguntas da pesquisa com atenção para que você possa obter sua recompensa em tokens em dinheiro.\nSe apressar, você será desqualificado.";
			
				bidText = "3 vencedores selecionados aleatoriamente entre os melhores licitantes";
				bidText2 = "Licite mais maçãs para ter maiores chances de ganhar!";
				
				string[] tips3 = {"Fique conectado para receber maçãs grátis durante o jogo",
                                "Convide seus amigos e ganhe recompensas em dinheiro",
                                "Preso em um palco ?, clique no botão 'pular' no canto superior direito",
                                "Está com sorte? Dê uma volta na roda da fortuna e ganhe prêmios!",
                                 "Maçãs verdes são mais frequentes à medida que seu estágio aumenta"};
				
				FruitUI.tips = tips3;
				
				
			break;
			
			case "italian":
			
				play = "Giocare";
				leaderboard = "Leaderboard";
				exit = "Uscita";
				earn = "GUADAGNA DI PIÙ!";
				infoText = "I giocatori che raccolgono il maggior numero di mele verdi vengono ricompensati ogni giorno! \n\nAssicurati che i tuoi dati Internet siano attivi durante il gioco per registrare il tuo punteggio. \n\nI vincitori vengono premiati all'inizio del giorno successivo entro le 00:00 (GMT) e la classifica viene ripristinata per il nuovo giorno. \n\nBuona fortuna!";
				welcome = "Benvenuto";
				next = "Il prossimo >>>";
				skip = "Salta >>";
				tap = "(Tocca per continuare)";
				tutorial0 = "Lascia che ti spieghi un breve tutorial su come funziona ...";
				tutorial1 = "Gioca";
				tutorial2 = "Spara e raccogli gettoni in contanti";
				tutorial3 = "Raccogli le mele";
				tutorial4 = "Usa le mele rosse per competere nei tornei giornalieri e vincere denaro";
				tutorial5 = "I giocatori che raccolgono il maggior numero di mele verdi vincono denaro ogni giorno!";
				tutorial5b = "(Puoi visualizzare le statistiche giornaliere su Leaderboard)";
				tutorial6 = "Una volta raggiunta la soglia di pagamento, puoi incassare tramite PayPal / conto bancario";
				tutorial6b = "(Il ritiro viene elaborato entro 2-8 giorni lavorativi)";
				tutorial7 = "Invita i tuoi amici e ricevi denaro quando si registrano";
				tutorial7b = "(Vieni pagato quando qualcuno accede utilizzando Facebook e inserisce il tuo codice di riferimento)";
				tutorial8 = "Puoi sempre guardare di nuovo questo tutorial ogni volta che vuoi";
				tutorial9 = "Ok, penso che sia tutto ciò che devi sapere ...";
				tutorial9b = "Accedi per iniziare";
				invite1 = "Ottieni "+inviteReward+" contanti per ogni 2 amici che inviti !!!";
				invite2 = "1. Gli inviti devono registrarsi utilizzando facebook \n2. Più attivi sono i tuoi inviti, maggiore sarà il tuo premio di invito (e viceversa)";
				invite3 = " più amici per ottenere ricompense in denaro";
				withdrawal1 = "Converti i tuoi gettoni in denaro in denaro reale";
				withdrawal2 = "(Il prelievo viene elaborato entro 2-8 giorni lavorativi)";
				withdrawal3 = "Inserisci la tua email Paypal";
				withdrawal4 = "I pagamenti verranno effettuati sul ";
				surveyText = "Leggi e rispondi con attenzione alle domande del sondaggio in modo da poter ottenere la ricompensa in gettoni di denaro.\nAffrettandoti verrai squalificato.";
			
				bidText = "3 Vincitori selezionati a caso tra i migliori offerenti";
				bidText2 = "Offri più mele per maggiori possibilità di vincita!";
				
				
				string[] tips4 = {"Resta connesso per ricevere mele gratis durante il gioco",
                                "Invita i tuoi amici e guadagna premi in denaro",
                                "Bloccato su un palco ?, fai clic sul pulsante 'Salta' in alto a destra",
                                "Ti senti fortunato? Fai un giro al volante della fortuna e vinci premi!",
                                 "Le mele verdi sono più frequenti con l'aumentare della fase"};
				
				FruitUI.tips = tips4;
				
			
			break;
			
			case "spanish":
			
				play = "Tocar";
				leaderboard = "Leaderboard";
				exit = "Salida";
				earn = "¡GANAR MAS!";
				infoText = "¡Los jugadores que recolecten la mayor cantidad de manzanas verdes son recompensados ​​diariamente! \n\nAsegúrese de que sus datos de Internet estén activados cuando jueguen para registrar su puntaje. \n\nLos ganadores son recompensados ​​al comienzo del día siguiente a las 12:00 a.m. (GMT) y la tabla de clasificación se restablece para el nuevo día. \n\n¡Buena suerte!";
				welcome = "Bienvenido";
				next = "Próximo >>>";
				skip = "Saltar >>";
				tap = "(Pulse para continuar)";
				tutorial0 = "Déjame darte un tutorial rápido de cómo funciona esto ...";
				tutorial1 = "Jugar un juego";
				tutorial2 = "Dispara y recoge fichas de efectivo";
				tutorial3 = "Recoger manzanas";
				tutorial4 = "Utilice manzanas rojas para competir en torneos diarios y ganar dinero";
				tutorial5 = "¡Los jugadores que recogen la mayoría de las manzanas verdes ganan dinero diariamente!";
				tutorial5b = "(Puede ver las estadísticas diarias en la tabla de líderes)";
				tutorial6 = "Una vez que alcanza el umbral de pago, puede retirar dinero a través de PayPal / Cuenta bancaria";
				tutorial6b = "(El retiro se procesa dentro de 2-8 días hábiles)";
				tutorial7 = "Invita a tus amigos y recibe dinero en efectivo cuando se registren";
				tutorial7b = "(Te pagan cuando alguien inicia sesión usando Facebook y pone tu código de referencia)";
				tutorial8 = "Siempre puedes volver a ver este tutorial cuando quieras";
				tutorial9 = "Está bien, creo que eso es todo lo que necesitas saber ...";
				tutorial9b = "Inicie sesión para comenzar";
				invite1 = "¡Consigue "+inviteReward+" efectivo por cada 2 amigos que invitas!";
				invite2 = "1. Las invitaciones deben registrarse usando facebook \n2. Cuanto más activas sean tus invitaciones, más recompensa será tu invitación (y viceversa)";
				invite3 = " Más amigos para obtener recompensa en efectivo";
				withdrawal1 = "Convierta sus tokens de efectivo en dinero real";
				withdrawal2 = "(El retiro se procesa dentro de 2-8 días hábiles)";
				withdrawal3 = "Ingrese su correo electrónico de Paypal";
				withdrawal4 = "Los pagos se realizarán el ";
				surveyText = "Lea y responda las preguntas de la encuesta detenidamente para que pueda obtener su recompensa de fichas en efectivo.\nSi se apresura, quedará descalificado.";
			
				bidText = "3 ganadores seleccionados al azar entre los mejores postores";
				bidText2 = "¡Oferta más manzanas para tener más posibilidades de ganar!";
				
				
				string[] tips5 = {"Mantente conectado para recibir manzanas gratis durante el juego",
                                "Invita a tus amigos y gana recompensas en efectivo",
                                "¿Atascado en un escenario ?, haz clic en el botón 'omitir' en la parte superior derecha",
                                "¿Te sientes afortunado? ¡Da una vuelta en la rueda de la fortuna y gana premios!",
                                 "Las manzanas verdes son más frecuentes a medida que aumenta tu etapa"};
				
				FruitUI.tips = tips5;
				
			
			break;
			
			case "filipino":
			
				play = "Maglaro";
				leaderboard = "Leaderboard";
				exit = "Exit";
				earn = "KUMITA PA PA!";
				infoText = "Ang mga manlalaro na nangongolekta ng pinaka berdeng mga mansanas ay gagantimpalaan araw-araw! \n\n Tiyaking nakabukas ang iyong data sa internet kapag nagpe-play upang maitala ang iyong iskor. \n\nMga gantimpala ang mga nanalo sa simula ng susunod na araw ng 00:00 (GMT) at ang leaderboard ay na-reset para sa bagong araw. \n\nMagandang kapalaran!Ang mga manlalaro na nangongolekta ng pinaka berdeng mga mansanas ay gagantimpalaan araw-araw! \n\nTiyaking nakabukas ang iyong data sa internet kapag nagpe-play upang maitala ang iyong iskor. \n\nMga gantimpala ang mga nanalo sa simula ng susunod na araw ng 00:00 (GMT) at ang leaderboard ay na-reset para sa bagong araw. \n\nMagandang kapalaran!";
				welcome = "Maligayang pagdating";
				next = "Susunod >>>";
				skip = "Laktawan >>";
				tap = "(Tapikin upang Magpatuloy)";
				tutorial0 = "Hayaan mo akong mailagay ka sa isang mabilis na tutorial kung paano ito gumagana ...";
				tutorial1 = "Maglaro";
				tutorial2 = "Shoot at mangolekta ng Cash Tokens";
				tutorial3 = "Kolektahin ang mga mansanas";
				tutorial4 = "Gumamit ng mga Pulang Mansanas upang Makipagkumpitensya sa Pang-araw-araw na Paligsahan at Manalo ng Pera";
				tutorial5 = "Ang mga manlalaro na nangongolekta ng pinaka berdeng mga mansanas ay nanalo ng pera araw-araw!";
				tutorial5b = "(Maaari mong tingnan ang Daily Stats sa Leaderboard)";
				tutorial6 = "Kapag naabot mo na ang threshold ng pagbabayad maaari kang mag-cash out sa pamamagitan ng PayPal / Bank Account";
				tutorial6b = "(Pinoproseso ang pag-withdraw sa loob ng 2-8 araw na may trabaho)";
				tutorial7 = "Anyayahan ang iyong mga kaibigan at makatanggap ng cash kapag nag-sign up sila";
				tutorial7b = "(Bayaran ka kapag nag-log in ang isang tao gamit ang facebook at inilalagay ang iyong referral code)";
				tutorial8 = "Maaari mong Palaging Manood ulit ang Tutorial na ito Sa anumang oras na gusto mo";
				tutorial9 = "Okay, sa palagay ko iyon lang ang kailangan mong malaman ...";
				tutorial9b = "Mag-log In upang magsimula";
				invite1 = "Kumuha ng "+inviteReward+" cash para sa bawat 2 kaibigan na iniimbitahan mo !!!";
				invite2 = "1. Dapat mag-sign up ang mga paanyaya gamit ang facebook \n2. Kung mas aktibo ang iyong mga imbitasyon, mas maraming reward ang iyong imbitasyon (at vice versa)";
				invite3 = " mas maraming kaibigan upang makakuha ng gantimpalang cash";
				withdrawal1 = "I-convert ang iyong mga cash token sa totoong pera";
				withdrawal2 = "(Pinoproseso ang pag-withdraw sa loob ng 2-8 araw na may pasok)";
				withdrawal3 = "Ipasok ang iyong email sa Paypal";
				withdrawal4 = "Magagawa ang mga pagbabayad sa ";
				surveyText = "Basahin at sagutin nang mabuti ang mga katanungan sa survey upang makuha mo ang gantimpala ng iyong mga token sa cash.\nAng pag-rushing sa iyo ay magdidiskuwalipika sa iyo.";
			
				bidText = "3 Mga Nanalong napili nang sapalaran mula sa nangungunang mga bidder";
				bidText2 = "Mag-bid ng Higit pang mga mansanas para sa Mas Mataas na Pagkakataon ng Panalong!";
				
				
				string[] tips6 =  {"Mantente conectado para recibir manzanas gratis durante el juego",
                                "Invita a tus amigos y gana recompensas en efectivo",
                                "¿Atascado en un escenario ?, haz clic en el botón 'omitir' en la parte superior derecha",
                                "¿Te sientes afortunado? ¡Da una vuelta en la rueda de la fortuna y gana premios!",
                                 "Las manzanas verdes son más frecuentes a medida que aumenta tu etapa"};
				
				FruitUI.tips = tips6;
			
			break;
			
			case "german":
				
				play = "Abspielen";
				leaderboard = "Leaderboard";
				exit = "Ausgang";
				earn = "VERDIENEN SIE MEHR!";
				infoText = "Spieler, die die meisten grünen Äpfel sammeln, werden täglich belohnt! \n\nStellen Sie sicher, dass Ihre Internetdaten beim Spielen aktiviert sind, um Ihre Punktzahl aufzuzeichnen. \n\nDie Gewinner werden zu Beginn des nächsten Tages bis 12:00 Uhr (GMT) und belohnt Die Rangliste wird für den neuen Tag zurückgesetzt. \n\nGenglück!";
				welcome = "Herzlich willkommen";
				next = "Nächster >>>";
				skip = "Überspringen >>";
				tap = "(Tippen um fortzufahren)";
				tutorial0 = "Lassen Sie mich Ihnen ein kurzes Tutorial zeigen, wie das funktioniert ...";
				tutorial1 = "Spiel spielen";
				tutorial2 = "Schieße und sammle Geldmarken";
				tutorial3 = "Sammle Äpfel";
				tutorial4 = "Verwenden Sie rote Äpfel, um an täglichen Turnieren teilzunehmen und Geld zu gewinnen";
				tutorial5 = "Spieler, die die meisten grünen Äpfel sammeln, gewinnen täglich Geld!";
				tutorial5b = "(Sie können die täglichen Statistiken auf dem Leaderboard anzeigen.)";
				tutorial6 = "Sobald Sie die Zahlungsschwelle erreicht haben, können Sie über PayPal / Bankkonto auszahlen";
				tutorial6b = "(Die Auszahlung erfolgt innerhalb von 2-8 Werktagen)";
				tutorial7 = "Laden Sie Ihre Freunde ein und erhalten Sie Bargeld, wenn sie sich anmelden";
				tutorial7b = "(Sie werden bezahlt, wenn sich jemand über Facebook anmeldet und Ihren Empfehlungscode eingibt.)";
				tutorial8 = "Sie können dieses Tutorial jederzeit wieder ansehen";
				tutorial9 = "Okay, ich denke das ist alles was du wissen musst ...";
				tutorial9b = "Melden Sie sich an, um zu beginnen";
				invite1 = "Holen Sie sich "+inviteReward+" Bargeld für jeweils 2 Freunde, die Sie einladen !!!";
				invite2 = "1. Einladungen müssen sich mit Facebook \n2. Je aktiver Ihre Einladungen sind, desto höher ist Ihre Einladungsbelohnung (und umgekehrt)";
				invite3 = " mehr Freunde, um Geldbelohnung zu bekommen";
				withdrawal1 = "Wandeln Sie Ihre Geldmarken in echtes Geld um";
				withdrawal2 = "(Die Auszahlung erfolgt innerhalb von 2-8 Werktagen)";
				withdrawal3 = "Geben Sie Ihre Paypal-E-Mail-Adresse ein";
				withdrawal4 = "Zahlungen werden am ";
				surveyText = "Lesen und beantworten Sie die Fragen der Umfrage sorgfältig, damit Sie Ihre Cash-Token-Belohnung erhalten.\nWenn Sie sich durchsetzen, werden Sie disqualifiziert.";
			
				bidText = "3 Gewinner, die zufällig aus den Top-Bietern ausgewählt wurden";		
				bidText2 = "Bieten Sie mehr Äpfel für eine höhere Gewinnchance!";
				
				string[] tips7 = {"Bleiben Sie in Verbindung, um während des Spiels kostenlose Äpfel zu erhalten",
                                "Laden Sie Ihre Freunde ein und verdienen Sie Geldprämien",
                                "Auf einer Bühne festgefahren? Klicken Sie oben rechts auf die Schaltfläche 'Überspringen'",
                                "Fühlen Sie sich glücklich? Machen Sie eine Spritztour am Glücksrad und gewinnen Sie Preise!",
                                 "Grüne Äpfel treten häufiger auf, wenn Ihr Stadium zunimmt."};
				
				FruitUI.tips = tips7;
				
				
			break;
		}
	}
}
