#include "individuo.h"
#include "parameters.h"
#include <vector>
using namespace std;

class Ambiente
{
public:

	Ambiente(void);
	~Ambiente(void);

	vector<Individuo*> selecionar(vector<Individuo*>);

};

