class Zadanie
{
	Układ układ;
	Test[] testy;
	void Rozwiaz(StreamReader tr)
	{
		int przypadek = 1;
		while (true)
		{
			Console.WriteLine($"\n\n\n\n\nDane nr {przypadek}");
			układ = new Układ();
			układ.Wczytaj(tr);
			int ileT = int.Parse(tr.ReadLine());
			testy = new Test[ileT];
			Console.WriteLine($"Ile testów: {ileT}");
			for (int i = 0; i < testy.Length; i++)
			{
				testy[i] = new Test();
			}

			układ.flaga_symulacja = true;
			for (int i = 0; i < testy.Length; i++)
			{
				układ.reset_bufora();
				testy[i].Wczytaj(tr, układ.ileWe, układ.ileWy);
				układ.aktualizuj_bufor(testy[i]);
				układ.symulacja(testy[i]);
			}
			if (układ.flaga_symulacja == true)
			{
				Console.WriteLine($"Case {przypadek}: No faults detected\n");
			}
			else
			{
				int nr_pops_bramki = -2, rodzaj_popsucia = -2;
				Console.WriteLine("\nCoś nie działa poprawnie testuję uszkodzenia bramek\n\nOdwracanie\n");
				for (int i = 0; i < układ.ileBramek; i++)
				{
					układ.flaga_symulacja = true;
					układ.bramki[i].Odwroc();
					Console.WriteLine("Odwrocenie bramki nr: " + (i + 1));
					for (int j = 0; j < testy.Length; j++)
					{
						układ.reset_bufora();
						układ.aktualizuj_bufor(testy[j]);
						układ.symulacja(testy[j]);
					}
					układ.bramki[i].Zdrowa();

					if (układ.flaga_symulacja == true)
					{
						Console.WriteLine("Odwrocenie bramki nr: " + (i + 1) + " naprawia układ.");
						if (nr_pops_bramki == -2)
						{
							nr_pops_bramki = i; rodzaj_popsucia = 2;
						}
						else
						{
							nr_pops_bramki = -1; rodzaj_popsucia = -1;
							break;
						}
					}
					Console.WriteLine();
				}

				Console.WriteLine("\nZawsze 1\n");
				if (nr_pops_bramki != -1)
				{
					for (int i = 0; i < układ.ileBramek; i++)
					{
						układ.flaga_symulacja = true;
						układ.bramki[i].Zawsze1();
						Console.WriteLine("Stala wartosc 1 dla bramki nr: " + (i + 1));
						for (int j = 0; j < testy.Length; j++)
						{
							układ.reset_bufora();
							układ.aktualizuj_bufor(testy[j]);
							układ.symulacja(testy[j]);
						}
						układ.bramki[i].Zdrowa();

						if (układ.flaga_symulacja == true)
						{
							Console.WriteLine("Stala wartosc 1 dla bramki nr: " + (i + 1) + " naprawia układ.");
							if (nr_pops_bramki == -2)
							{
								nr_pops_bramki = i; rodzaj_popsucia = 1;
							}
							else
							{
								nr_pops_bramki = -1; rodzaj_popsucia = -1;
								break;
							}
						}
						Console.WriteLine();
					}
				}

				if (nr_pops_bramki != -1)
				{
					Console.WriteLine("\nZawsze 0\n");
					for (int i = 0; i < układ.ileBramek; i++)
					{
						układ.flaga_symulacja = true;
						układ.bramki[i].Zawsze0();
						Console.WriteLine("Stala wartosc 0 dla bramki nr: " + (i + 1));
						for (int j = 0; j < testy.Length; j++)
						{
							układ.reset_bufora();
							układ.aktualizuj_bufor(testy[j]);
							układ.symulacja(testy[j]);
						}
						układ.bramki[i].Zdrowa();

						if (układ.flaga_symulacja == true)
						{
							Console.WriteLine("Stala wartosc 0 dla bramki nr: " + (i + 1) + " naprawia układ.");
							if (nr_pops_bramki == -2)
							{
								nr_pops_bramki = i; rodzaj_popsucia = 0;
							}
							else
							{
								nr_pops_bramki = -1; rodzaj_popsucia = -1;
								break;
							}
						}
						Console.WriteLine();
					}
				}


				if (nr_pops_bramki == -1)
				{
					Console.WriteLine($"Case {przypadek}: Unable to totally classify the failure");
				}
				else if (rodzaj_popsucia == 2)
				{
					Console.WriteLine($"Case {przypadek}: Gate {nr_pops_bramki + 1} is failing; output inverted");
				}
				else if (rodzaj_popsucia == 1)
				{
					Console.WriteLine($"Case {przypadek}: Gate {nr_pops_bramki + 1} is failing; output stuck at 1");
				}
				else if (rodzaj_popsucia == 0)
				{
					Console.WriteLine($"Case {przypadek}: Gate {nr_pops_bramki + 1} is failing; output stuck at 0");
				}


			}
			przypadek++;
		}


	}
	static void Main(string[] args)
	{
		Zadanie zadanie = new Zadanie();
		StreamReader tr = new StreamReader("dane.TXT");
		zadanie.Rozwiaz(tr);
	}

};


public class Test
{
	public int[] inputs, outputs;

	public void Wczytaj(StreamReader tr, int ileWe, int ileWy)
	{
		inputs = new int[ileWe];
		outputs = new int[ileWy];
		string string_temp = tr.ReadLine();
		Console.WriteLine(string_temp);
		string[] strlist_temp = string_temp.Split(" ");
		for (int i = 0; i < ileWe; i++)
		{
			inputs[i] = Convert.ToInt32(strlist_temp[i]);
		}
		for (int i = ileWe; i < ileWy + ileWe; i++)
		{
			outputs[i - ileWe] = Convert.ToInt32(strlist_temp[i]);
		}


	}

};

public class Układ
{
	public Bramka_w_ukladzie[] wejscia, bramki, wyjscia;

	public int ileWe, ileBramek, ileWy;
	public bool flaga_symulacja;
	public void Wczytaj(StreamReader tr)
	{

		string string1 = tr.ReadLine();
		string[] strlist = string1.Split(" ", StringSplitOptions.RemoveEmptyEntries);
		ileWe = int.Parse(strlist[0]);
		ileBramek = int.Parse(strlist[1]);
		ileWy = int.Parse(strlist[2]);
		if (ileWe == 0 && ileBramek == 0 && ileWy == 0)
		{
			Environment.Exit(0);
		}
		Console.WriteLine($"Ilość wejść: {ileWe}");
		Console.WriteLine($"Ilość bramek: {ileBramek}");
		Console.WriteLine($"Ilość wyjść: {ileWy}");
		wejscia = new Bramka_w_ukladzie[ileWe];
		bramki = new Bramka_w_ukladzie[ileBramek];
		wyjscia = new Bramka_w_ukladzie[ileWy];

		for (int i = 0; i < wejscia.Length; i++)
		{
			wejscia[i] = new Bramka_w_ukladzie();
		}
		for (int i = 0; i < bramki.Length; i++)
		{
			bramki[i] = new Bramka_w_ukladzie();
		}
		for (int i = 0; i < wyjscia.Length; i++)
		{
			wyjscia[i] = new Bramka_w_ukladzie();
		}




		//dla kazdej bramki odczytac "wejscia" (skąd pochodzą sygnały)
		for (int i = 0; i < bramki.Length; i++)
		{
			//referencja na poprzednie bramki
			string string2 = tr.ReadLine();
			Console.WriteLine(string2);
			string[] strlist2 = string2.Split(" ");
			if (strlist2[0] == "n")
			{
				bramki[i].setTypBramki(0); //0 not, 1 or, 2 xor, 3 and
			}
			else if (strlist2[0] == "o")
			{
				bramki[i].setTypBramki(1);

			}
			else if (strlist2[0] == "x")
			{
				bramki[i].setTypBramki(2);
			}
			else if (strlist2[0] == "a")
			{
				bramki[i].setTypBramki(3);
			}

			Bramka_w_ukladzie temp_bramka1 = null, temp_bramka2 = null;
			if (strlist2[1][0] == 'i')
			{
				temp_bramka1 = wejscia[Convert.ToInt32(Char.GetNumericValue(strlist2[1][1])) - 1];
				if (bramki[i].getTypBramki() == 0)
				{
					temp_bramka2 = wejscia[Convert.ToInt32(Char.GetNumericValue(strlist2[1][1])) - 1];
					//bramka not
				}
				else
				{
					temp_bramka2 = wejscia[Convert.ToInt32(Char.GetNumericValue(strlist2[2][1])) - 1];
				}


			}
			else if (strlist2[1][0] == 'g')
			{

				temp_bramka1 = bramki[Convert.ToInt32(Char.GetNumericValue(strlist2[1][1])) - 1];
				if (bramki[i].getTypBramki() == 0)
				{
					temp_bramka2 = bramki[Convert.ToInt32(Char.GetNumericValue(strlist2[1][1])) - 1];
					//bramka not
				}
				else
				{
					temp_bramka2 = wejscia[Convert.ToInt32(Char.GetNumericValue(strlist2[2][1])) - 1];
				}

			}

			bramki[i].Ustaw_Wejscia_Bramek(temp_bramka1, temp_bramka2);
		}
		//każdemu wyjsciu odpowiada jedna bramka
		string string3 = tr.ReadLine();
		string[] strlist3 = string3.Split(" ");
		for (int i = 0; i < wyjscia.Length; i++)
		{
			wyjscia[i].set_przed_wyjsciem_id(int.Parse(strlist3[i]) - 1);
			//wyjscia[i].Ustaw_Wejscia_Wyjsc(bramki[int.Parse(strlist3[i])-1],int.Parse(strlist3[i]));
		}



	}
	public void reset_bufora()
	{
		for (int i = 0; i < wejscia.Length; i++)
		{
			wejscia[i].Ustaw_Bufor(-1);
		}
		for (int i = 0; i < bramki.Length; i++)
		{
			bramki[i].Ustaw_Bufor(-1);
		}
		for (int i = 0; i < wyjscia.Length; i++)
		{
			wyjscia[i].Ustaw_Bufor(-1);
		}

	}
	public void aktualizuj_bufor(Test test1)
	{
		for (int i = 0; i < test1.inputs.Length; i++)
		{
			wejscia[i].Ustaw_Bufor(test1.inputs[i]);
		}
	}
	public void symulacja(Test test1)
	{

		for (int i = 0; i < test1.outputs.Length; i++)
		{
			int wynik_z_danych = test1.outputs[i];
			int wynik_symulacji = bramki[wyjscia[i].get_przed_wyjsciem_id()].Output_Bramki();
			//temp_wynik_symulacji = wyjscia[i].Output_Bramki();
			if (wynik_z_danych != wynik_symulacji)
			{
				flaga_symulacja = false;
			}
			Console.WriteLine("Output " + (i + 1) + " z danych: " + wynik_z_danych + " Output " + (i + 1) + " z symulacji: " + wynik_symulacji);
			//Console.WriteLine(wyjscia[0].Output_Bramki());
		}
	}
}


class BramkaNot : Bramka
{
	public int rodzaj_bramki; //0 not, 1 or, 2 xor, 3 and, 4 wyjscie
	public override int get_rodzaj_bramki()
	{
		return rodzaj_bramki;
	}

	public BramkaNot()
	{
		rodzaj_bramki = 0;
	}
	public override bool wynik_bramki(bool wynik_poprz1, bool wynik_poprz2)
	{

		return !wynik_poprz1;

	}
}
class BramkaOr : Bramka
{
	public int rodzaj_bramki; //0 not, 1 or, 2 xor, 3 and, 4 wyjscie

	public BramkaOr()
	{
		rodzaj_bramki = 1;
	}
	public override int get_rodzaj_bramki()
	{
		return rodzaj_bramki;
	}
	public override bool wynik_bramki(bool wynik_poprz1, bool wynik_poprz2)
	{

		return wynik_poprz1 | wynik_poprz2;

	}
}
class BramkaXor : Bramka
{
	public int rodzaj_bramki; //0 not, 1 or, 2 xor, 3 and, 4 wyjscie

	public BramkaXor()
	{
		rodzaj_bramki = 2;
	}

	public override int get_rodzaj_bramki()
	{
		return rodzaj_bramki;
	}
	public override bool wynik_bramki(bool wynik_poprz1, bool wynik_poprz2)
	{

		return wynik_poprz1 ^ wynik_poprz2;

	}
}
class BramkaAnd : Bramka
{
	public int rodzaj_bramki; //0 not, 1 or, 2 xor, 3 and, 4 wyjscie

	public BramkaAnd()
	{
		rodzaj_bramki = 3;
	}
	public override int get_rodzaj_bramki()
	{
		return rodzaj_bramki;
	}
	public override bool wynik_bramki(bool wynik_poprz1, bool wynik_poprz2)
	{
		return wynik_poprz1 & wynik_poprz2;
	}
}
class BramkaWyjscie : Bramka
{
	public int rodzaj_bramki; //0 not, 1 or, 2 xor, 3 and, 4 wyjscie
	public BramkaWyjscie()
	{
		rodzaj_bramki = 4;
	}
	public override int get_rodzaj_bramki()
	{
		return rodzaj_bramki;
	}
	public override bool wynik_bramki(bool wynik_poprz1, bool wynik_poprz2)
	{
		return wynik_poprz1;
	}
}

public class Bramka
{
	//rodzaje bramek
	int rodzaj_bramki; //0 not, 1 or, 2 xor, 3 and, 4 wyjscie

	/*
	public Bramka(int rodz_bramk)
	{
		rodzaj_bramki = rodz_bramk;
	}
	*/
	public virtual int get_rodzaj_bramki()
	{
		return -1;
	}
	public virtual bool wynik_bramki(bool wynik_poprz1, bool wynik_poprz2)
	{
		if (rodzaj_bramki == 0)
		{
			return !wynik_poprz1;
		}
		else if (rodzaj_bramki == 1)
		{
			return wynik_poprz1 | wynik_poprz2;
		}
		else if (rodzaj_bramki == 2)
		{
			return wynik_poprz1 ^ wynik_poprz2;
		}
		else if (rodzaj_bramki == 3)
		{
			return wynik_poprz1 & wynik_poprz2;
		}
		return wynik_poprz1; //wyjscie
	}

};

public class Bramka_w_ukladzie
{
	Bramka_w_ukladzie[] poprzednie_bramki;
	Bramka bramka1;
	int bufor_output;
	int typ_bramki;

	int przed_wyjsciem_id;


	bool zdrowa_br, odwroc, zawsz0, zawsz1;

	public void set_przed_wyjsciem_id(int set)
	{
		przed_wyjsciem_id = set;
	}
	public int get_przed_wyjsciem_id()
	{
		return przed_wyjsciem_id;
	}
	public Bramka_w_ukladzie()
	{
		poprzednie_bramki = new Bramka_w_ukladzie[2];
		poprzednie_bramki[0] = null;
		poprzednie_bramki[1] = null;
		zdrowa_br = true;
		odwroc = false;
		zawsz0 = false;
		zawsz1 = false;

	}
	public void setTypBramki(int typ)
	{
		typ_bramki = typ;
		if (typ == 0)
		{
			bramka1 = new BramkaNot();
		}
		else if (typ == 1)
		{
			bramka1 = new BramkaOr();
		}
		else if (typ == 2)
		{
			bramka1 = new BramkaXor();
		}
		else if (typ == 3)
		{
			bramka1 = new BramkaAnd();
		}
		else if (typ == 4)
		{
			bramka1 = new BramkaWyjscie();
		}
		//0 not, 1 or, 2 xor, 3 and, 4 wyjscie
	}
	public int getTypBramki()
	{
		return bramka1.get_rodzaj_bramki();

	}
	public void Ustaw_Bufor(int wartosc)//przydatne dla inputow
	{
		bufor_output = wartosc;
	}

	public void Ustaw_Wejscia_Bramek(Bramka_w_ukladzie poprz_bramk1, Bramka_w_ukladzie poprz_bramk2)
	{
		bufor_output = -1;
		poprzednie_bramki[0] = poprz_bramk1;
		poprzednie_bramki[1] = poprz_bramk2;
	}


	public void Zdrowa()
	{
		zdrowa_br = true;
		odwroc = false;
		zawsz0 = false;
		zawsz1 = false;
	}
	public void Odwroc()
	{
		zdrowa_br = false;
		odwroc = true;
		zawsz0 = false;
		zawsz1 = false;
	}
	public void Zawsze0()
	{
		zdrowa_br = false;
		odwroc = false;
		zawsz0 = true;
		zawsz1 = false;
	}
	public void Zawsze1()
	{
		zdrowa_br = false;
		odwroc = false;
		zawsz0 = false;
		zawsz1 = true;
	}
	public void Policz_Output_Bramki()
	{
		int output1, output2;
		//Console.WriteLine("Probuje2: "+Convert.ToBoolean(poprzednie_bramki[0].Output_Bramki())+ Convert.ToBoolean(poprzednie_bramki[1].Output_Bramki()));
		bufor_output = Convert.ToInt32(bramka1.wynik_bramki(Convert.ToBoolean(poprzednie_bramki[0].Output_Bramki()), Convert.ToBoolean(poprzednie_bramki[1].Output_Bramki())));
	}
	public int Output_Bramki()
	{
		if (bufor_output == -1)
		{
			//Console.WriteLine("Probuje");
			Policz_Output_Bramki();

			//bufor_output = 1;
		}
		//Console.WriteLine("Output jaki wyszedl: " + bufor_output);



		if (odwroc == true)
			return bufor_output == 1 ? 0 : 1;
		if (zawsz0 == true)
			return 0;
		if (zawsz1 == true)
			return 1;
		return bufor_output;
	}
};



