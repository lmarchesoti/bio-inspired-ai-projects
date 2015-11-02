#include "individuo_impl.h"
#include "evolutor.h"
#include "ambiente.h"
#include <vector>
using namespace std;

class AG
{
private:

	vector<Individuo*> populacao;
	Evolutor evolutor;
	Ambiente ambiente;

public:

	AG(void);
	~AG(void);

	void crossOver();

	void mutacao();

	void normalizacao();

	void resultado();
	
	void rodarGeracao();

	void executaAG();

};

