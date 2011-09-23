<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<fieldset class="instructions">
    <legend>Справка</legend>
    <div class="introduction">
        Для упрощения написания новостей были введены (будут продолжать появляться) некоторые сокращеные конструкции. 
        Используя которые можно вставлять в новость изображения или спец знаки:
    </div>
    <p>
        <ul>
            <li><span class="shortcut">&lt;cut&gt;</span> - Разрезать новость на краткую и подробную части;</li>
            <li><span class="shortcut">&lt;zerg&gt; (или &lt;зерг&gt;)</span> - Вставить иконку <img src="/Content/img/races/zerg.png" alt="Зерг" />Зергов;</li>
            <li><span class="shortcut">&lt;protoss&gt; (или &lt;протосс&gt;)</span> - Вставить иконку <img src="/Content/img/races/protoss.png" alt="Протосс" />Протоссов;</li>
            <li><span class="shortcut">&lt;terran&gt; (или &lt;терран&gt;)</span> - Вставить иконку <img src="/Content/img/races/terran.png" alt="Терран" />Терран;</li>
            <li><span class="shortcut">&lt;random&gt; (или &lt;случайная&gt;)</span> - Вставить иконку <img src="/Content/img/races/random.png" alt="Случайная" />Случайной расы;</li>
            <li><span class="shortcut">&lt;zerg1&gt; (или &lt;зерг1&gt;)</span> - Вставить иконку <img src="/Content/img/races/zerg1.png" alt="Зерг" />Зергов SC:BW;</li>
            <li><span class="shortcut">&lt;protoss1&gt; (или &lt;протосс1&gt;)</span> - Вставить иконку <img src="/Content/img/races/protoss1.png" alt="Протосс" />Протоссов SC:BW;</li>
            <li><span class="shortcut">&lt;terran1&gt; (или &lt;терран1&gt;)</span> - Вставить иконку <img src="/Content/img/races/terran1.png" alt="Терран" />Терран SC:BW;</li>
            <li><span class="shortcut">&lt;gold&gt;</span> - Вставить иконку <img src="/Content/img/medals/gold.gif" alt="Золотая медаль" />Золотой медали;</li>
            <li><span class="shortcut">&lt;silver&gt;</span> - Вставить иконку <img src="/Content/img/medals/silver.gif" alt="Серебряная медаль" />Серебряной медали;</li>
            <li><span class="shortcut">&lt;bronze&gt;</span> - Вставить иконку <img src="/Content/img/medals/bronze.gif" alt="Бронзовая медаль" />Бронзовой медали;</li>
        </ul>
    </p>
</fieldset>