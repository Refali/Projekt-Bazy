   M S B A M L   `   `   `  ����_  [Projekt_Czarnacka_Gawron_Hasa_Kuchta, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null[2clr-namespace:Projekt_Czarnacka_Gawron_Hasa_Kuchta$Projekt_Czarnacka_Gawron_Hasa_Kuchta  5   	   1    +Projekt_Czarnacka_Gawron_Hasa_Kuchta.Lekarz   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Dlocal2clr-namespace:Projekt_Czarnacka_Gawron_Hasa_Kuchta     5   	     � Title$  Lekarz��5   	   $	��464��6   $��670.594��6%   ���	#FFFBFAD3 � Icon$ logo.png��6L   $��352.594��6\   $	��458��6o    � WindowStartupLocation$ CenterScreen=�6   .��5	      � .+�5
   
   �� $��Zalogowano jako:��6   $��33,0,0,20.4q�6+    K� ZIndex$
 1000��6@   $
��Lato)�6T   $
��Bold"�6f   $
��Left=�6x   $	��104��6�   $��24��6�   $��Bottom=�6�   �� 5   
   -   5         ��Name$ userLbl��5      $�� ��6!   $
��Left=�6,   $��142,0,0,20.4q�6G   $
 1000��6]   $��95��6q   $
��Lato)�6|   $��24��6�   $��Bottom=�6�   ]� 5   
   .+�5      � -   5         �Name$ PanelEdycjaWizyty��5      $	��657��6.   $ 1��6:   $y�Visible=�6K   ���	#FFFBFAD3$��
0,2,0,-1.6q�6w   .+�5      &   System.Windows.Controls.DataGrid  -   5          Name$ 
WizytyView��5      $	��235��6/   $��
28,99,26,0q�6<   $	��Top=�6P      AutoGenerateColumns .     CanUserAddRows .  	   CanUserDeleteRows	 .  $
��Lato)�6�   $
��Auto��6�   
   Columns
 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Id��6-      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 69   �� *id+6P     5      $ Pacjent��6-    .  6>   �� *	Pacjent+6[     5      $
 Data��6-    .  6;   �� *data+ �� StringFormat$ 	yyy-MM-dd��6n     5      $ Godzina��6-    .  6>   �� *	godzina+6Z     5      $ Status��6-    .  6=   �� *status+6X     5      $ Uwagi��6-    6<   �� *uwagi+5      �� 5      -   5         ��Name$ ZapiszEdycje��5      $��
AKTUALIZUJ��6/   $
��Left=�6D   $��267,339,0,0q�6_   $	��Top=�6t   $	��114��6�   ���	#FFC6E377$
��Lato)�6�   $��35��6�   $��16(�6�   �� 5      -   5         ��Name$ txtWyszukaj��5      $
��Left=�6/   $��34��6J   $��	28,50,0,0q�6V    �� TextWrapping$
 Wrap=�6i   $	��Top=�6}   $	��158��6�   6�   �� 5      $��	Wyszukaj:��6   $
��Left=�6,   $��	28,25,0,0q�6G   $	��Top=�6Z   $	��158��6r   $
��Lato)�6~   �� 5      -   5        $ 
btnWyloguj��5      $��WYLOGUJ��6-   $
��Left=�6?   $��
545,50,0,0q�6Z   $	��Top=�6n   $��86��6�   $��34��6�   ���	#FFC6E377$
��Lato)�6�   $��14(�6�   �� 5      -   5        $ Label_WizytyZmiany��5      $#��Zmiany nie zostały zapisane!��64   $
��Left=�6\   $��457,391,0,0q�6w   $	��Top=�6�   $	��190��6�   $
��Lato)�6�   $��32��6�   $��14(�6�   
���  ��$y�Hidden=�6  ��6�   Z� 6  5      5      5       5!      