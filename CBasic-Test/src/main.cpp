#include "cbstd.h"

int main()
{
	cb_string drink = "Mountain Dew";
	u32 drinklen = cb_strlen(drink);

	cb_print(drink);
	cb_print("String Length: " + cb_tostr(drinklen));

	cb_replace(drink, "Pepsi");
	drinklen = cb_strlen(drink);

	cb_print(drink);
	cb_print("String Length: " + cb_tostr(drinklen));

	cb_breakpoint("Program ended!");

	return 0;
}