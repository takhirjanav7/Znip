# Znip

Znip — IT mutaxassislari uchun mo'ljallangan, ko'nikmalarga asoslangan ijtimoiy tarmoq backend tizimi. Foydalanuvchilar profil yaratishi, postlar ulashishi, bir-birini follow qilishi va real vaqtda xabar almashishi mumkin.

## Asosiy funksiyalar

* **Autentifikatsiya** — Google OAuth orqali kirish, JWT access va refresh token tizimi
* **Postlar** — foydalanuvchilar post yaratishi, ko'rishi va boshqarishi
* **Follow tizimi** — foydalanuvchilarni kuzatish (follow/unfollow)
* **Real-time chat** — SignalR orqali jonli xabar almashish
* **Skill-based qidiruv** — foydalanuvchilarni ko'nikmalari bo'yicha topish

## Texnologiyalar

* **Backend:** C# / .NET
* **Real-time aloqa:** SignalR
* **Autentifikatsiya:** Google OAuth, JWT (access + refresh token)
* **Ma'lumotlar bazasi:** *PostgreSQL*

## Loyiha tuzilishi

```
AsosiyProject/
├── src/           # asosiy manba kodi
├── ...
```

## O'rnatish va ishga tushirish

```bash
git clone https://github.com/takhirjanav7/Znip.git
cd Znip
dotnet restore
dotnet run
```

## Muallif

Loyiha [Abdulaziz](https://github.com/takhirjanav7) tomonidan ishlab chiqilmoqda.

