/* Auramix Panel - Anasayfa Dashboard Grafikleri */

(function () {
    'use strict';

    var AylikSatisChart = function () {
        var el = document.getElementById('chart_aylik_satis');
        if (!el || typeof d3 === 'undefined') return;

        var dataStr = (typeof _dashboardAylikId !== 'undefined' && document.getElementById(_dashboardAylikId)) ?
            document.getElementById(_dashboardAylikId).value : '';
        if (!dataStr) return;

        var data;
        try { data = JSON.parse(dataStr); } catch (e) { return; }
        if (!data || !data.length) return;

        var parseDate = d3.time.format('%Y-%m-%d').parse;
        data.forEach(function (d) {
            d.date = parseDate(d.date);
            d.value = +d.value;
        });

        var margin = { top: 20, right: 20, bottom: 40, left: 50 };
        var container = d3.select(el);
        var width = el.getBoundingClientRect().width - margin.left - margin.right;
        var height = 260 - margin.top - margin.bottom;

        var x = d3.time.scale().range([0, width]);
        var y = d3.scale.linear().range([height, 0]);
        var area = d3.svg.area()
            .x(function (d) { return x(d.date); })
            .y0(height)
            .y1(function (d) { return y(d.value); })
            .interpolate('monotone');

        x.domain(d3.extent(data, function (d) { return d.date; }));
        y.domain([0, d3.max(data, function (d) { return d.value; }) || 1]);

        var svg = container.append('svg')
            .attr('width', width + margin.left + margin.right)
            .attr('height', height + margin.top + margin.bottom)
            .append('g')
            .attr('transform', 'translate(' + margin.left + ',' + margin.top + ')');

        svg.append('path')
            .datum(data)
            .attr('class', 'd3-area')
            .style('fill', '#2196F3')
            .style('fill-opacity', 0.6)
            .attr('d', area);

        var xAxis = d3.svg.axis().scale(x).orient('bottom')
            .ticks(6)
            .tickFormat(d3.time.format('%b %y'));
        svg.append('g')
            .attr('class', 'd3-axis d3-axis-bottom')
            .attr('transform', 'translate(0,' + height + ')')
            .call(xAxis);

        var yAxis = d3.svg.axis().scale(y).orient('left')
            .ticks(5)
            .tickFormat(function (d) { return d >= 1000 ? (d / 1000) + 'k' : d; });
        svg.append('g')
            .attr('class', 'd3-axis d3-axis-y')
            .call(yAxis);
    };

    var SiparisDonutChart = function () {
        var el = document.getElementById('chart_siparis_donut');
        if (!el || typeof d3 === 'undefined' || typeof d3.tip === 'undefined') return;

        var dataStr = (typeof _dashboardSiparisId !== 'undefined' && document.getElementById(_dashboardSiparisId)) ?
            document.getElementById(_dashboardSiparisId).value : '';
        if (!dataStr) return;

        var data;
        try { data = JSON.parse(dataStr); } catch (e) { return; }
        if (!data || !data.length) return;

        data = data.map(function (d) { return { label: d.label, value: Math.max(1, d.value) }; });
        var sum = d3.sum(data, function (d) { return d.value; });
        var size = 220;
        var radius = (size / 2) - 2;

        var pie = d3.layout.pie()
            .sort(null)
            .value(function (d) { return d.value; });
        var arc = d3.svg.arc()
            .outerRadius(radius)
            .innerRadius(radius / 2);
        var colors = ['#4CAF50', '#2196F3', '#FF9800', '#9C27B0', '#F44336', '#00BCD4'];
        var colorScale = d3.scale.ordinal().range(colors);

        var tip = d3.tip()
            .attr('class', 'd3-tip')
            .offset([-10, 0])
            .html(function (d) {
                var pct = sum > 0 ? ((d.value / sum) * 100).toFixed(1) : 0;
                return '<strong>' + d.data.label + '</strong><br/>' + d.value + ' sipariş (' + pct + '%)';
            });

        var svg = d3.select(el).append('svg')
            .attr('width', size)
            .attr('height', size)
            .call(tip)
            .append('g')
            .attr('transform', 'translate(' + (size / 2) + ',' + (size / 2) + ')');

        var arcGroup = svg.selectAll('.d3-arc')
            .data(pie(data))
            .enter()
            .append('g')
            .attr('class', 'd3-arc')
            .style('cursor', 'pointer');

        arcGroup.append('path')
            .attr('d', arc)
            .style('fill', function (d) { return colorScale(d.data.label); })
            .on('mouseover', tip.show)
            .on('mouseout', tip.hide);
    };

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', function () {
            setTimeout(function () {
                AylikSatisChart();
                SiparisDonutChart();
            }, 100);
        });
    } else {
        setTimeout(function () {
            AylikSatisChart();
            SiparisDonutChart();
        }, 100);
    }
})();
