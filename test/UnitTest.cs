using System.Collections;
using System.Collections.Generic;
using GameController;

public class UnitTest {
    
    public boolean testSaneDeckInit() {
        /**
         * Desc: Test that initialization of decks is sane.
         *
         * Returns: True, pass
         *          False, fail
         */
        GameController test = new GameController();  // Init Mock object
        test.Start();

        // Check the decks are correctly initialized
        Queue<int> p1 = test.getP1Deck();
        Queue<int> p2 = test.getP2Deck();

        if(p1.Count + p2.Count != 52) {
            return false;
        }

        // Check if the decks contain the correct types of cards
        int[] deckFiller = new int[52];
        for(int i = 0; i < 51; i++) {
            deckFiller[i] = -1;
        }

        int card;
        try {
            while(card = p1.Dequeue()) {
                deckFiller[card]++;
            }
        } catch (Exception e) {}

        try {
            while(card = p1.Dequeue()) {
                deckFiller[card]++;
            }

        } catch(Exception e) {}

        for(int i = 0; i < 51; i++) {
            if(deckFiller[i] == -1 || deckFiller[i] > 0) {
                return false;
            }
        }

        return true;
    }
}
