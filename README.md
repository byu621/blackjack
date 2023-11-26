# Blackjack

## 0. Dealer BJ Ev
What is player Ev given the dealer has blackjack? \
[data](data/DealerBlackjack.csv)


## 1. Dealer Total
What are the odds the dealer ends up with `17` `18` `19` `20` `21` or `bust` given their current hand? 
[data](data/DealerTotal.csv)

## 2. Player Stand Ev
What is player Ev given the player stands? [data](data/PlayerStandEv.csv)

## 3. Player Hit Ev
What is player Ev given the player can hit or stand? [data](data/PlayerHitEv.csv)

## Rules
- S17 -> Dealer stands on soft 17
- 3:2 -> Blackjack pays 3 to 2
- 6 Deck -> 6 decks are used

## Glossary
- Hard -> If a hand has an ace, it's classed as a hard hand when the ace counts as 1
- Soft -> If a hand has an ace, it's classed as a soft hand when the ace counts as 11
- Push -> the case when the dealer and the player have the same value and they neither win nor lose
- P(BJ) -> 4/13 * 1/13 * 2 = 0.047 = 4.7%
- P(BJ | A) -> 4/13 = 0.307 = 30.7%
- P(BJ | 10) ->  1/13 = 0.076 = 7.6%
- Ev -> Expected Value e.g. An Expected Value of 50% means you are expected to gain $0.50 for each $1 bet