using System;
using System.Runtime.InteropServices;
using SFML.Learning;
using SFML.System;
using SFML.Window;

namespace CardGame
{
    internal class Program : Game
    {
        static string[] iconsName;

        static int[,] cards;
        static int cardCount = 20;
        static int cardWight = 100;
        static int cardHight = 100;

        static int countPerLine = 5;
        static int space = 40;
        static int leftOffset = 70;
        static int topOffset = 20;

        static string cardClick = LoadSound("card_click.wav");
        static string cardCoup = LoadSound("card_coup.wav");

        static void LoadIcons()
        {
            iconsName = new string[7];

            iconsName[0] = LoadTexture("cardBackground.png");

            for(int i = 1; i < iconsName.Length; i++)
            {
                iconsName[i] = LoadTexture("Icon_" + (i).ToString() + ".png");
            }

        }

        static void Shuffle(int[] arr) 
        {
            Random rand= new Random();

            for(int i = arr.Length - 1; i >= 1; i--)
            {
                int j = rand.Next(i, i + 1);

                int tmp = arr[j];
                arr[j] = arr[i];
                arr[i] = tmp;
            }
        }

        static void InitCard()
        {
            Random rnd = new Random();
            cards = new int[cardCount, 6];
            
            int id = 0;
            int[] iconId = new int[cards.GetLength(0)];

            for(int i = 0; i < iconId.Length; i++)
            {
                if (i % 2 == 0)
                {
                    id = rnd.Next(1, 7);
                }

                iconId[i] = id;
            }

            Shuffle(iconId);

            for (int i = 0; i < cards.GetLength(0);  i++)
            {
                cards[i, 0] = 0;
                cards[i, 1] = (i % countPerLine) * (cardWight + space) + leftOffset ;
                cards[i, 2] = (i / countPerLine) * (cardHight + space) + topOffset;
                cards[i, 3] = cardWight;
                cards[i, 4] = cardHight;
                cards[i, 5] = iconId[i];
            }

        }

        static void SetStateToAllCards(int state)
        {
            for (int i = 0; i < cards.GetLength(0); i++)
            {
                cards[i, 0] = state;
            }
        }

        static void DrawCards()
        {
            for (int i = 0; i < cards.GetLength(0); i++)
            {
                if (cards[i, 0] == 1)// open
                {
                    DrawSprite(iconsName[cards[i, 5]], cards[i, 1], cards[i, 2]);
                } 

                if (cards[i, 0] == 0)// close
                {
                    DrawSprite(iconsName[0], cards[i, 1], cards[i, 2]);
                } 
            }
        }

        static int GetIndexCardByMousePosititon()
        {
            for (int i = 0; i < cards.GetLength(0); i++)
            {
                if (MouseX >= cards[i, 1] && MouseX <= cards[i, 1] + cards[i, 3] && MouseY >= cards[i, 2] && MouseY <= cards[i, 2] + cards[i, 4])
                {
                    return i;
                }
            }

            return -1;
        }

        static void Main(string[] args)
        {
            LoadIcons();

            SetFont("Nevduplenysh-Regular.otf");

            InitWindow(800, 600, "Find Couple");

            int openCardAmount = 0;
            int firstOpenCardIndex = -1;
            int secondOpenCardIndex = -1;
            int remainingCard = cardCount;

            InitCard();
            
            SetStateToAllCards(1);
            
            ClearWindow(26, 46, 92);
            DrawCards();
            DisplayWindow();

            Delay(5000);
            SetStateToAllCards(0);

            while (true)
            {
                DispatchEvents();

                if (remainingCard == 0)
                {
                    break;
                }

                if (openCardAmount == 2)
                {
                    if (cards[firstOpenCardIndex, 5] == cards[secondOpenCardIndex, 5])
                    {
                        cards[firstOpenCardIndex, 0] = -1;
                        cards[secondOpenCardIndex, 0] = -1;

                        remainingCard -= 2;

                        PlaySound(cardCoup);
                    }
                    else
                    {
                        cards[firstOpenCardIndex, 0] = 0;
                        cards[secondOpenCardIndex, 0] = 0;
                    }
                    firstOpenCardIndex = -1;
                    secondOpenCardIndex = -1;
                    openCardAmount = 0;

                    Delay(1000);
                }

                if (GetMouseButtonDown(0) == true)
                {
                    PlaySound(cardClick);

                    int index = GetIndexCardByMousePosititon();

                    if (index != -1 && index != firstOpenCardIndex)
                    {
                        cards[index, 0] = 1;
                       
                        openCardAmount++;

                        if (openCardAmount == 1) firstOpenCardIndex = index;
                        if (openCardAmount == 2) secondOpenCardIndex = index;
                    }
                }

                ClearWindow(26, 46, 92);

                DrawCards();

                DisplayWindow();

                Delay(1);
            }
            ClearWindow();

            SetFillColor(255, 255, 255);
            DrawText(300, 250, "Вы открыли все карты !", 36);

            DisplayWindow();

            Delay(5000);
        }
    }
}
