
using System;
using System.Collections.Generic;
namespace DeepSpace
{

	class Estrategia
	{
		
		
		public String Consulta1( ArbolGeneral<Planeta> arbol)
		{
			Planeta BotPlaneta = PlanetaBot(arbol);
			List<Planeta> ListaCamino = new List<Planeta>();
			List<string> team = new List<string> ();
			camino(arbol,BotPlaneta,ListaCamino);
			foreach ( Planeta p in ListaCamino )
			{
				if (p.EsPlanetaDeLaIA())
				{
					team.Add("Planeta del Bot");
				}
				if (p.EsPlanetaDelJugador())
				{
					team.Add("Planeta del juegador");
				}
				if (p.EsPlanetaNeutral())
				{
					team.Add("Planeta Neutral");
				}
			}
			string resultado = string.Join(" -> ", team);
			
			return resultado;
		}

		public String Consulta2( ArbolGeneral<Planeta> arbol)
		{
			if (arbol.getDatoRaiz()==null){
				return "El arbol esta vacio";
			}
			ArbolGeneral<Planeta> Bot = PlanetaBot1(arbol);
			
			
			string resultado= "Descendientes del Bot: " +"\n"+  calcularDescendientes(Bot) ;
			
			
			return resultado;
		}
	
	
		public String Consulta3( ArbolGeneral<Planeta> arbol)
		{
			Cola<ArbolGeneral<Planeta>> cola = new Cola<ArbolGeneral<Planeta>>();
			ArbolGeneral<Planeta> arbolAux;
			
			int nivel=0;
			//int poblacionTotal = CalcularPoblacion(arbol);
		    int poblacionNivel=0;
		    int cantHijosPorNivel=0;
		    string resultado = "";
		    
		    
			cola.encolar(arbol);
			cola.encolar(null);
			Console.WriteLine("\nNivel: {0}",nivel);
			
			while(!cola.esVacia())
			{
				arbolAux= cola.desencolar();
				if (arbolAux==null){
				resultado += " Población Total del Nivel " + nivel + " = " +  poblacionNivel + ", Promedio de poblacion por planeta = " + poblacionNivel/cantHijosPorNivel+ "\n";
				if(!cola.esVacia()){
					cola.encolar(null);
				
				    nivel++;

				}
					poblacionNivel = 0;
					cantHijosPorNivel = 0;
			}
			else{
				
			poblacionNivel = poblacionNivel + arbolAux.getDatoRaiz().Poblacion();
			cantHijosPorNivel = cantHijosPorNivel + 1; 
			
			if(arbolAux.getHijos()!=null){
			foreach(var hijo in arbolAux.getHijos())
			{
				cola.encolar(hijo);
			}
			}
			}
		}
			return resultado;
			}
		
		public Movimiento CalcularMovimiento(ArbolGeneral<Planeta> arbol)
		{
			ArbolGeneral<Planeta> p = PlanetaBot1(arbol);
			
			List<ArbolGeneral<Planeta>> PlanetasBot = new List<ArbolGeneral<Planeta>>();
			ObtenerPlanetasDelBot(arbol,PlanetasBot);
			ArbolGeneral<Planeta> PlanetaActual=p;
			
			
			foreach(ArbolGeneral<Planeta> nodo in PlanetasBot)
			{
				
				if (!TodosHijosConquistados(nodo))
				{
					
					Planeta planeta = EncontrarPlanetaEnemigo(nodo);
					Movimiento movimiento = new Movimiento(nodo.getDatoRaiz(),planeta);
					return movimiento;
				}
				
			}
			
			foreach(ArbolGeneral<Planeta> nodo in PlanetasBot)
			{
				ArbolGeneral<Planeta> Padre = PlanetaPadre(arbol,nodo,null);
				if (Padre!=null && Padre.getDatoRaiz().EsPlanetaDeLaIA() == false)
				{
					Movimiento mov = new Movimiento(nodo.getDatoRaiz(),Padre.getDatoRaiz());
					return mov;
				}
			}
			
        return null;
    }
		// ----------------------- Funcion auxiliares ---------------------- //
		
		private ArbolGeneral<Planeta> PlanetaPadre(ArbolGeneral<Planeta> actual,ArbolGeneral<Planeta> arbolBuscado,ArbolGeneral<Planeta> anterior)
		{
			if (actual == null)
        {
            return null; // No se encontró el nodo buscado
        }

        if (actual.Equals(arbolBuscado))
        {
            return anterior; // Se encontró el nodo buscado, devolver el nodo anterior
        }

        foreach (var hijo in actual.getHijos())
        {
            ArbolGeneral<Planeta> resultado = PlanetaPadre(hijo, arbolBuscado, actual);
            if (resultado != null)
            {
                return resultado; // Si se encuentra en algún nivel inferior, devolver el resultado
            }
        }

        return null; // No se encontró en este subárbol
    }
		
		private string calcularDescendientes(ArbolGeneral<Planeta> arbolBot,string prefijo = "")
		{
			string resultado="";	
			if (arbolBot.getDatoRaiz().EsPlanetaDeLaIA())
    {
				resultado += prefijo +"Planeta del bot ("+arbolBot.getDatoRaiz().Poblacion()+")\n";
    }
		if (arbolBot.getDatoRaiz().EsPlanetaDelJugador())
    {
        resultado += prefijo +"Planeta del Jugador ("+arbolBot.getDatoRaiz().Poblacion()+")\n";
    }
		if (arbolBot.getDatoRaiz().EsPlanetaNeutral())
    {
        resultado += prefijo +"Planeta Neutral ("+arbolBot.getDatoRaiz().Poblacion()+")\n";
    }

    // Agregar prefijo para representar la jerarquía
    prefijo += "  ";

    foreach (var hijo in arbolBot.getHijos())
    {
    	resultado += calcularDescendientes(hijo,prefijo);
    }

    return resultado;
		
    }
		private int CalcularPoblacion(ArbolGeneral<Planeta> arbol)
		{
			int poblacionArbol = arbol.getDatoRaiz().Poblacion();
			
			foreach( var hijo in arbol.getHijos())
			{
				poblacionArbol = poblacionArbol + CalcularPoblacion(hijo);
			}
			
			return poblacionArbol;
		}
		
		private Planeta EncontrarPlanetaEnemigo(ArbolGeneral<Planeta> arbol)
    {
		Planeta objetivo = null;
		ArbolGeneral<Planeta> planetaAcutal= arbol;
           foreach (var nodo in planetaAcutal.getHijos())
              {
			
           	if (nodo.getDatoRaiz().EsPlanetaDeLaIA() == false )
                {
				if (objetivo==null || nodo.getDatoRaiz().Poblacion() < objetivo.Poblacion())
					
					objetivo = nodo.getDatoRaiz();
                }   
        }
          
           
        // Si no se encuentra ningún planeta enemigo en el árbol, retorna null
        return objetivo;
    }
		private bool TodosHijosConquistados(ArbolGeneral<Planeta> arbol)
    {
			foreach (var hijo in arbol.getHijos())
        {
				if (hijo.getDatoRaiz().EsPlanetaDeLaIA()==false)
            {
                return false; // Al menos un hijo del Bot no ha sido conquistado
            }
        }
		
        return true; // Todos los hijos del Bot han sido conquistados
    }
		
	
    
        private void ObtenerPlanetasDelBot(ArbolGeneral<Planeta> arbol, List<ArbolGeneral<Planeta>> planetasDelBot)
    {
        if (arbol.getDatoRaiz().EsPlanetaDeLaIA()) 	
        {
            planetasDelBot.Add(arbol);
        }

        foreach (var hijo in arbol.getHijos())
        {
            ObtenerPlanetasDelBot(hijo, planetasDelBot);
        }
    }
		
		
		private Planeta PlanetaBot(ArbolGeneral<Planeta> arbol)
		{
			if ( arbol.getDatoRaiz() == null)
			{
				return null;
			}
			if (arbol.getDatoRaiz().EsPlanetaDeLaIA())
			{
				return arbol.getDatoRaiz();
			}
			
			foreach(var hijo in arbol.getHijos())
			{
				var resultado = PlanetaBot(hijo);
				if (resultado != null)
				{
					return resultado;
				}
			}
			return null;
		}
		private ArbolGeneral<Planeta> PlanetaBot1(ArbolGeneral<Planeta> arbol)
		{
			if ( arbol.getDatoRaiz() == null)
			{
				return null;
			}
			if (arbol.getDatoRaiz().EsPlanetaDeLaIA())
			{
				return arbol;
			}
			
			foreach(var hijo in arbol.getHijos())
			{
				ArbolGeneral<Planeta> resultado = PlanetaBot1(hijo);
				if (resultado != null)
				{
					return resultado;
				}
			}
			return null;
		}
		private bool camino(ArbolGeneral<Planeta> arbol, Planeta planeta, List<Planeta> listaCamino)
		{
			
			bool caminoEncontrado = false;
			if (arbol == null)
			{
				return false;
			}
			listaCamino.Add(arbol.getDatoRaiz());
			
			if (arbol.getDatoRaiz().Equals(planeta))
			{
				return true;
			}
			foreach(var planetahijo in arbol.getHijos())
			{
				caminoEncontrado = camino(planetahijo,planeta,listaCamino);
				if ( caminoEncontrado)
				{
					return true;
				}
			}
			listaCamino.RemoveAt(listaCamino.Count-1);
			return false;
		}
		
	}
}
