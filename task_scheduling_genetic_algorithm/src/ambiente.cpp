#include "ambiente.h"
#include <algorithm>
using namespace std;

Ambiente::Ambiente(void)
{
}


Ambiente::~Ambiente(void)
{
}

struct pointer_less {
    template< typename T >
    bool operator()( T const *lhs, T const *rhs ) const
        { return * lhs < * rhs; }
};

#ifdef ELITISMO_

vector<Individuo*> Ambiente::selecionar(vector<Individuo*> populacao){
	sort(populacao.begin(), populacao.end(), & compare);
	while(populacao.size() > POPSIZE){
		populacao.pop_back();
	}
	return populacao;
}

#endif
