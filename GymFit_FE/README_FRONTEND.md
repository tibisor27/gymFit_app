# GymFit Frontend

Frontend-ul pentru aplicația GymFit - un sistem de management pentru săli de fitness.

## 🚀 Tehnologii Folosite

- **React 19** cu TypeScript
- **Material-UI (MUI)** pentru componente UI
- **Redux Toolkit** pentru state management
- **React Router** pentru navigare
- **Axios** pentru API calls
- **Vite** pentru build tool

## 📁 Structura Proiectului

```
src/
├── api/                    # API configuration și funcții
│   ├── api.config.ts      # Configurare Axios
│   ├── auth.api.ts        # API calls pentru autentificare
│   └── users.api.ts       # API calls pentru utilizatori
├── components/            # Componente React
│   ├── auth/             # Componente pentru autentificare
│   │   ├── LoginForm.tsx
│   │   └── RegisterForm.tsx
│   ├── users/            # Componente pentru utilizatori
│   │   ├── UserList.tsx
│   │   └── UserForm.tsx
│   └── common/           # Componente comune
│       ├── Navbar.tsx
│       └── ProtectedRoute.tsx
├── hooks/                # Custom hooks
│   └── redux.ts          # Typed Redux hooks
├── pages/                # Pagini principale
│   ├── Dashboard.tsx
│   └── UsersPage.tsx
├── store/                # Redux store
│   ├── index.ts          # Store configuration
│   ├── authSlice.ts      # Auth state management
│   └── usersSlice.ts     # Users state management
├── types/                # TypeScript types
│   ├── auth.types.ts
│   └── user.types.ts
└── App.tsx               # Componenta principală
```

## 🔧 Instalare și Rulare

### Prerequisite
- Node.js (versiunea 18+)
- npm sau yarn
- Backend-ul .NET rulând pe `https://localhost:7000`

### Pași de instalare

1. **Clonează repository-ul** (dacă nu l-ai făcut deja)
```bash
git clone <repository-url>
cd gymfit-frontend
```

2. **Instalează dependențele**
```bash
npm install
```

3. **Configurează URL-ul backend-ului**
Verifică în `src/api/api.config.ts` că URL-ul backend-ului este corect:
```typescript
const API_URL = 'https://localhost:7000'; // URL-ul backend-ului tău .NET
```

4. **Rulează aplicația în modul development**
```bash
npm run dev
```

Aplicația va fi disponibilă la `http://localhost:5173`

## 🔐 Funcționalități Implementate

### Autentificare
- **Login** - Autentificare cu email și parolă
- **Register** - Înregistrare utilizatori noi cu validare
- **Logout** - Deconectare și ștergere token
- **Protected Routes** - Rute protejate care necesită autentificare

### Gestionarea Utilizatorilor
- **Vizualizare utilizatori** - Listă cu toți utilizatorii
- **Creare utilizatori** - Formular pentru utilizatori noi
- **Editare utilizatori** - Actualizare informații utilizatori
- **Ștergere utilizatori** - Ștergere cu confirmare
- **Filtrare după rol** - Member, Trainer, Admin

### Dashboard
- **Statistici rapide** - Numărul de membri, traineri, etc.
- **Navigare rapidă** - Acces rapid la funcționalități
- **Design responsive** - Funcționează pe toate dispozitivele

## 🔌 Integrare cu Backend-ul

Frontend-ul se conectează la backend-ul .NET prin următoarele endpoint-uri:

### Auth Endpoints
- `POST /auth/login` - Autentificare
- `POST /auth/register` - Înregistrare

### Users Endpoints
- `GET /users` - Lista utilizatorilor
- `GET /users/{id}` - Utilizator specific
- `POST /users` - Creare utilizator
- `PATCH /users/{id}` - Actualizare utilizator
- `DELETE /users/{id}` - Ștergere utilizator

## 🎨 Componente Principale

### LoginForm
- Formular de autentificare
- Validare în timp real
- Gestionarea erorilor
- Redirect după login

### RegisterForm
- Formular de înregistrare
- Validare complexă (parolă, vârstă, telefon)
- Mesaje de succes/eroare
- Redirect la login după înregistrare

### UserList
- Afișare utilizatori în grid
- Acțiuni CRUD (Create, Read, Update, Delete)
- Filtrare și căutare
- Confirmare pentru ștergere

### UserForm
- Formular pentru creare/editare utilizatori
- Validare pe frontend
- Selectare rol utilizator
- Gestionarea stărilor de loading

## 🔄 State Management

Aplicația folosește Redux Toolkit pentru gestionarea stării:

### Auth State
```typescript
interface AuthState {
  token: string | null;
  isAuthenticated: boolean;
  loading: boolean;
  error: string | null;
}
```

### Users State
```typescript
interface UsersState {
  users: UserDTO[];
  currentUser: UserDTO | null;
  loading: boolean;
  error: string | null;
}
```

## 🚦 Rutele Aplicației

- `/` - Redirect la dashboard
- `/login` - Pagina de login
- `/register` - Pagina de înregistrare
- `/dashboard` - Dashboard principal (protejat)
- `/users` - Gestionarea utilizatorilor (protejat)

## 🔧 Configurare Avansată

### Modificarea URL-ului Backend
În `src/api/api.config.ts`:
```typescript
const API_URL = 'https://your-backend-url.com';
```

### Adăugarea de noi endpoint-uri
1. Definește tipurile în `src/types/`
2. Creează funcțiile API în `src/api/`
3. Adaugă Redux slice în `src/store/`
4. Creează componentele în `src/components/`

### Customizarea temei Material-UI
În `src/App.tsx`:
```typescript
const theme = createTheme({
  palette: {
    primary: {
      main: '#your-color',
    },
  },
});
```

## 🐛 Debugging

### Probleme comune

1. **CORS Errors**
   - Verifică configurarea CORS în backend
   - Asigură-te că URL-ul backend-ului este corect

2. **401 Unauthorized**
   - Token-ul a expirat - fă login din nou
   - Verifică că token-ul se trimite în header

3. **Network Errors**
   - Backend-ul nu rulează
   - URL-ul backend-ului este greșit

### Logging
Pentru debugging, poți adăuga console.log în:
- Redux actions (`src/store/`)
- API calls (`src/api/`)
- Componente (`console.log` în useEffect)

## 📱 Responsive Design

Aplicația este optimizată pentru:
- **Desktop** - Layout complet cu sidebar
- **Tablet** - Layout adaptat cu meniuri colapsabile
- **Mobile** - Layout stack cu navigare touch-friendly

## 🔮 Funcționalități Viitoare

- [ ] Gestionarea antrenamentelor
- [ ] Calendar pentru programări
- [ ] Rapoarte și statistici
- [ ] Notificări push
- [ ] Chat între membri și traineri
- [ ] Plăți online
- [ ] Aplicație mobilă

## 🤝 Contribuții

Pentru a contribui la proiect:
1. Fork repository-ul
2. Creează o branch nouă (`git checkout -b feature/new-feature`)
3. Commit schimbările (`git commit -am 'Add new feature'`)
4. Push la branch (`git push origin feature/new-feature`)
5. Creează un Pull Request

## 📄 Licență

Acest proiect este licențiat sub MIT License. 