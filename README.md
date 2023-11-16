# Blackjack


## 1. Dealer Total
What are the odds the dealer ends up with `17` `18` `19` `20` `21` or `bust` given their current hand? 
[data](data/DealerTotal.csv)

## 2. Player Stand Win
What are the odds the player wins given the player stands? [data](data/PlayerStandWin.csv)

#### Dependencies
- 1\. Dealer Total

## 3. Player Stand Push
What are the odds the player pushes given the player stands? [data](data/PlayerStandPush.csv)

#### Dependencies
- 1\. Dealer Total

## Rules
- S17 -> Dealer stands on soft 17

## Glossary
- Hard -> If a hand has an ace, it's classed as a hard hand when the ace counts as 1
- Soft -> If a hand has an ace, it's classed as a soft hand when the ace counts as 11.
- Push -> the case when the dealer and the player have the same value and they neither win nor lose