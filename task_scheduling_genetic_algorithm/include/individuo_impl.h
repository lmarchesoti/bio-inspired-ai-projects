#include "individuo.h"
#define NTAREFAS 18
#define NPROC 2
#define COMCOST 8

struct IndividuoStruct{
	int tarefa;
	int processador;
};

class IndividuoImpl : public Individuo
{
private:
	int posicoes[NTAREFAS];
	vector<IndividuoStruct> escalonamento;

public:
	IndividuoImpl(void);
	IndividuoImpl(const IndividuoImpl&);
	~IndividuoImpl(void);

	void fav();

	void mutate();

	bool better(Individuo*);
	
	bool ehSolucao();

	void print();

	vector<Individuo*> cross(void*);

	void reparar();

	void troca(int, int);

	double custoCaminho(int);

};
