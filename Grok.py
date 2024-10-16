import grok
import datetime
import sqlite3
import os
import json
from werkzeug.utils import secure_filename
import hashlib

DATABASE = 'my_reptile_family.db'
UPLOAD_FOLDER = 'uploads/'
ALLOWED_EXTENSIONS = {'pdf', 'jpg', 'jpeg', 'png', 'doc', 'docx'}

if not os.path.exists(UPLOAD_FOLDER):
    os.makedirs(UPLOAD_FOLDER)

class MyReptileFamilyApp(grok.Application, grok.Container):
    def __init__(self):
        super().__init__()
        self.init_db()

    def init_db(self):
        with sqlite3.connect(DATABASE) as conn:
            cursor = conn.cursor()
            cursor.execute('''
                CREATE TABLE IF NOT EXISTS users (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    username TEXT UNIQUE NOT NULL,
                    email TEXT NOT NULL,
                    password TEXT NOT NULL
                )
            ''')
            cursor.execute('''
                CREATE TABLE IF NOT EXISTS reptiles (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT NOT NULL,
                    species TEXT NOT NULL,
                    morph TEXT,
                    birthdate TEXT,
                    parent_id INTEGER,
                    FOREIGN KEY (parent_id) REFERENCES reptiles (id)
                )
            ''')
            cursor.execute('''
                CREATE TABLE IF NOT EXISTS posts (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    username TEXT NOT NULL,
                    content TEXT NOT NULL,
                    timestamp TEXT NOT NULL
                )
            ''')
            cursor.execute('''
                CREATE TABLE IF NOT EXISTS documents (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    reptile_id INTEGER NOT NULL,
                    filename TEXT NOT NULL,
                    upload_date TEXT NOT NULL,
                    FOREIGN KEY (reptile_id) REFERENCES reptiles (id)
                )
            ''')

def allowed_file(filename):
    return '.' in filename and filename.rsplit('.', 1)[1].lower() in ALLOWED_EXTENSIONS

def hash_password(password):
    return hashlib.sha256(password.encode()).hexdigest()

class HomePage(grok.View):
    grok.context(MyReptileFamilyApp)
    grok.name('')
    
    def update(self):
        with sqlite3.connect(DATABASE) as conn:
            cursor = conn.cursor()
            cursor.execute('SELECT * FROM reptiles')
            self.reptiles = cursor.fetchall()
            cursor.execute('SELECT * FROM posts ORDER BY id DESC')
            self.posts = cursor.fetchall()

    def post_message(self, username, content):
        timestamp = datetime.datetime.now().strftime("%Y-%m-%d %H:%M:%S")
        with sqlite3.connect(DATABASE) as conn:
            cursor = conn.cursor()
            cursor.execute('INSERT INTO posts (username, content, timestamp) VALUES (?, ?, ?)', (username, content, timestamp))
        self.redirect(self.url(''))

class CollectionPage(grok.View):
    grok.context(MyReptileFamilyApp)
    grok.name('collection')
    
    def update(self):
        with sqlite3.connect(DATABASE) as conn:
            cursor = conn.cursor()
            cursor.execute('SELECT * FROM reptiles')
            self.reptiles = cursor.fetchall()

class ReptileFamilyDataAPI(grok.View):
    grok.context(MyReptileFamilyApp)
    grok.name('api/reptile_family_data')
    grok.require('zope.Public')
    
    def render(self):
        with sqlite3.connect(DATABASE) as conn:
            cursor = conn.cursor()
            cursor.execute('SELECT id, name, species, morph, birthdate, parent_id FROM reptiles')
            reptiles = cursor.fetchall()
            reptile_list = []
            for reptile in reptiles:
                reptile_list.append({
                    'id': reptile[0],
                    'name': reptile[1],
                    'species': reptile[2],
                    'morph': reptile[3],
                    'birthdate': reptile[4],
                    'parent_id': reptile[5]
                })
            return json.dumps(reptile_list)

class SalePage(grok.View):
    grok.context(MyReptileFamilyApp)
    grok.name('sale')
    
    def update(self):
        with sqlite3.connect(DATABASE) as conn:
            cursor = conn.cursor()
            cursor.execute('SELECT * FROM reptiles')
            self.reptiles = cursor.fetchall()

class LoginPage(grok.View):
    grok.context(MyReptileFamilyApp)
    grok.name('login')
    
    def update(self):
        if self.request.method == 'POST':
            username = self.request.form.get('username')
            password = hash_password(self.request.form.get('password'))
            with sqlite3.connect(DATABASE) as conn:
                cursor = conn.cursor()
                cursor.execute('SELECT * FROM users WHERE username = ? AND password = ?', (username, password))
                user = cursor.fetchone()
                if user:
                    self.redirect(self.url(''))
                else:
                    self.error_message = "Login Failed"

class RegisterPage(grok.View):
    grok.context(MyReptileFamilyApp)
    grok.name('register')
    
    def update(self):
        if self.request.method == 'POST':
            username = self.request.form.get('username')
            email = self.request.form.get('email')
            password = hash_password(self.request.form.get('password'))
            with sqlite3.connect(DATABASE) as conn:
                cursor = conn.cursor()
                try:
                    cursor.execute('INSERT INTO users (username, email, password) VALUES (?, ?, ?)', (username, email, password))
                    self.redirect(self.url('login'))
                except sqlite3.IntegrityError:
                    self.error_message = "Username already exists"

class AddReptile(grok.View):
    grok.context(MyReptileFamilyApp)
    grok.name('add_reptile')
    
    def update(self):
        if self.request.method == 'POST':
            name = self.request.form.get('name')
            species = self.request.form.get('species')
            morph = self.request.form.get('morph', '')
            birthdate = self.request.form.get('birthdate', '')
            parent_id = self.request.form.get('parent_id', None)
            with sqlite3.connect(DATABASE) as conn:
                cursor = conn.cursor()
                cursor.execute('INSERT INTO reptiles (name, species, morph, birthdate, parent_id) VALUES (?, ?, ?, ?, ?)', (name, species, morph, birthdate, parent_id))
            self.redirect(self.url('collection'))

class UploadDocument(grok.View):
    grok.context(MyReptileFamilyApp)
    grok.name('upload_document')
    
    def update(self, reptile_id):
        if self.request.method == 'POST':
            file = self.request.file.get('document')
            if file and allowed_file(file.filename):
                filename = secure_filename(file.filename)
                filepath = os.path.join(UPLOAD_FOLDER, filename)
                file.save(filepath)
                upload_date = datetime.datetime.now().strftime("%Y-%m-%d %H:%M:%S")
                with sqlite3.connect(DATABASE) as conn:
                    cursor = conn.cursor()
                    cursor.execute('INSERT INTO documents (reptile_id, filename, upload_date) VALUES (?, ?, ?)', (reptile_id, filename, upload_date))
                self.redirect(self.url('collection'))
            else:
                self.error_message = "Invalid file type"

class PurchasePage(grok.View):
    grok.context(MyReptileFamilyApp)
    grok.name('purchase')
    
    def update(self, reptile_id):
        with sqlite3.connect(DATABASE) as conn:
            cursor = conn.cursor()
            cursor.execute('SELECT * FROM reptiles WHERE id = ?', (reptile_id,))
            self.reptile = cursor.fetchone()
            if not self.reptile:
                self.error_message = "Reptile Not Found"