# ParseMusicScoreY

一款用于自动弹奏风物之诗琴的辅助程序

谱面示例如下:
```txt
625
PP/PP/PP/E TE /
(ZVW) (AQ) /(AFH)Q (AFW)/ (XB) /(SGE) TE /
(CNW) (DQ) /(DH)Q (CNQ)/ (ZB) /(AGE) (BT)E /
(ZVW) A /(AFE)T (AFT)/ Y(XB) /(SGT)R(BE) /
(CN) (DW)E/(DT)YE(CNW)/E (ZB) /(AGE) (BT)E /

(ZVW) (AQ) /(AFH)Q (AFW)/ (XB) /(SGE) TE /
(CNW) (DQ) /(DH)Q (CNQ)/ (ZB) /(AGQ)J(BH)J /
(ZVH) A /(AFW) (ZV)/J (XBH) /(SGJ) (BJ) /
(ZVN) Q /(XBM)P/(CNA)P/XCNA/
```
第一行为每一拍所需的时间，这个时间将保持到演奏结束，以"/"分隔的是每一拍所需的按键，括号内的代表同时按键。

简单来说，每次按键间隔为节拍时间除以当前节拍的按键组数。

比如: (DH)Q (CNQ),按键组数为3，每一拍时间为625ms，那么在处理时，DH键会被按下然后抬起，再过208ms，Q键会被按下然后抬起，再过208ms，CNQ键会被按下然后抬起，然后再等待208ms执行下一个节拍。

**注: 在谱面中虽然有P键，但在实际演奏中P键不会触发任何音符，此处作空白音符使用**