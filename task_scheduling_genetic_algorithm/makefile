CC=g++
TARGET=prog

ODIR=obj
IDIR=include
LDIR=lib
SDIR=src
FLAGS=-I$(IDIR)

_DEPS=ag.h ambiente.h evolutor.h individuo.h individuo_impl.h parameters.h
DEPS = $(patsubst %,$(IDIR)/%,$(_DEPS))

_OBJ=ag.o ambiente.o evolutor.o individuo.o individuo_impl.o main.o
OBJ = $(patsubst %,$(ODIR)/%,$(_OBJ))

$(ODIR)/%.o: src/%.cpp $(DEPS)
	$(CC) -c -o $@ $< $(FLAGS)

.PHONY=compile
compile c: $(OBJ)
	$(CC) -o $(TARGET) $^ $(FLAGS)

.PHONY=run
run r:
	./$(TARGET)

.PHONY=batch
batch b:
	sh $(LDIR)/batch.sh

.PHONY=clean
clean n:
	rm -f $(ODIR)/*.o *~ $(TARGET) $(IDIR)/*~

.PHONY=all
all a: clean compile run
